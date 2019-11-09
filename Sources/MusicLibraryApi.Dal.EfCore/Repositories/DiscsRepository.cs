using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
				.ProjectTo<Disc>(mapper.ConfigurationProvider)
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			return await DiscsWithIncludedProperties
				.Where(x => x.Id == discId)
				.ProjectTo<Disc>(mapper.ConfigurationProvider)
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
