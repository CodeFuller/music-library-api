using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using static System.FormattableString;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class DiscsRepository : IDiscsRepository
	{
		private readonly MusicLibraryDbContext context;

		private IQueryable<Disc> DiscsWithIncludedProperties =>
			context.Discs
				.Include(d => d.Songs).ThenInclude(s => s.Genre)
				.Include(d => d.Songs).ThenInclude(s => s.Artist);

		public DiscsRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<Disc> AddDisc(int folderId, Disc disc, CancellationToken cancellationToken)
		{
			var folder = await context.Folders.SingleOrDefaultAsync(f => f.Id == folderId, cancellationToken);
			if (folder == null)
			{
				throw new NotFoundException(Invariant($"The folder with id of {folderId} does not exist"));
			}

			disc.Folder = folder;

			context.Discs.Add(disc);
			await context.SaveChangesAsync(cancellationToken);

			return disc;
		}

		public async Task<IEnumerable<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return await DiscsWithIncludedProperties
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			return await DiscsWithIncludedProperties
				.Where(x => x.Id == discId)
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
