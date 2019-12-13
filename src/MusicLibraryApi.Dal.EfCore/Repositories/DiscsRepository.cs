using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using static System.FormattableString;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class DiscsRepository : IDiscsRepository
	{
		private readonly MusicLibraryDbContext context;

		public DiscsRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<int> CreateDisc(Disc disc, CancellationToken cancellationToken)
		{
			context.Discs.Add(disc);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException())
			{
				throw new FolderNotFoundException(Invariant($"The folder with id of {disc.FolderId} does not exist"));
			}

			return disc.Id;
		}

		public async Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return await context.Discs
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			return await context.Discs.Where(d => discIds.Contains(d.Id))
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			var disc = await context.Discs
				.Where(d => d.Id == discId)
				.SingleOrDefaultAsync(cancellationToken);

			if (disc == null)
			{
				throw new DiscNotFoundException(Invariant($"The disc with id of {discId} does not exist"));
			}

			return disc;
		}

		public async Task<IReadOnlyCollection<Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			return await context.Discs.Where(d => folderIds.Contains(d.FolderId))
				.ToListAsync(cancellationToken);
		}
	}
}
