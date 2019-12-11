using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Dal.EfCore.Entities;
using static System.FormattableString;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class DiscsRepository : IDiscsRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public DiscsRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreateDisc(Disc disc, CancellationToken cancellationToken)
		{
			var discEntity = mapper.Map<DiscEntity>(disc);
			context.Discs.Add(discEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException())
			{
				throw new FolderNotFoundException(Invariant($"The folder with id of {disc.FolderId} does not exist"));
			}

			return discEntity.Id;
		}

		public async Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return await context.Discs
				.Select(d => mapper.Map<Disc>(d))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			return await context.Discs.Where(d => discIds.Contains(d.Id))
				.Select(d => mapper.Map<Disc>(d))
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			var discEntity = await context.Discs
				.Where(d => d.Id == discId)
				.SingleOrDefaultAsync(cancellationToken);

			if (discEntity == null)
			{
				throw new DiscNotFoundException(Invariant($"The disc with id of {discId} does not exist"));
			}

			return mapper.Map<Disc>(discEntity);
		}

		public async Task<IReadOnlyCollection<Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			return await context.Discs.Where(d => folderIds.Contains(d.FolderId))
				.Select(s => mapper.Map<Disc>(s))
				.ToListAsync(cancellationToken);
		}
	}
}
