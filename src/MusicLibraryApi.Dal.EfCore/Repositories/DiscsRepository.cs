using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions;
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

		private IQueryable<DiscEntity> DiscsWithIncludedProperties =>
			context.Discs
				.Include(d => d.Songs).ThenInclude(s => s.Genre)
				.Include(d => d.Songs).ThenInclude(s => s.Artist);

		public DiscsRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> AddDisc(int folderId, Disc disc, CancellationToken cancellationToken)
		{
			var folder = await context.Folders.SingleOrDefaultAsync(f => f.Id == folderId, cancellationToken);
			if (folder == null)
			{
				throw new NotFoundException(Invariant($"The folder with id of {folderId} does not exist"));
			}

			var discEntity = mapper.Map<DiscEntity>(disc);
			discEntity.Folder = folder;

			context.Discs.Add(discEntity);
			await context.SaveChangesAsync(cancellationToken);

			return discEntity.Id;
		}

		public async Task<IEnumerable<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return await DiscsWithIncludedProperties
				.Select(d => mapper.Map<Disc>(d))
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			var discEntity = await DiscsWithIncludedProperties
				.Where(x => x.Id == discId)
				.SingleOrDefaultAsync(cancellationToken);

			if (discEntity == null)
			{
				throw new NotFoundException(Invariant($"The disc with id of {discId} does not exist"));
			}

			return mapper.Map<Disc>(discEntity);
		}
	}
}
