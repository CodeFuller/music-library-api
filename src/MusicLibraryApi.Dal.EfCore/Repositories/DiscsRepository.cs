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

		private IQueryable<DiscEntity> Discs => context.Discs.Include(d => d.Folder);

		public DiscsRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreateDisc(int folderId, Disc disc, CancellationToken cancellationToken)
		{
			var discEntity = mapper.Map<DiscEntity>(disc);
			discEntity.Folder = await context.FindFolder(folderId, cancellationToken);

			context.Discs.Add(discEntity);
			await context.SaveChangesAsync(cancellationToken);

			return discEntity.Id;
		}

		public async Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return await Discs
				.Select(d => mapper.Map<Disc>(d))
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			var discEntity = await Discs
				.Where(d => d.Id == discId)
				.SingleOrDefaultAsync(cancellationToken);

			if (discEntity == null)
			{
				throw new DiscNotFoundException(Invariant($"The disc with id of {discId} does not exist"));
			}

			return mapper.Map<Disc>(discEntity);
		}

		public async Task<IReadOnlyCollection<Disc>> GetFolderDiscs(int folderId, CancellationToken cancellationToken)
		{
			return await Discs.Where(d => d.Folder.Id == folderId)
				.Select(d => mapper.Map<Disc>(d))
				.ToListAsync(cancellationToken);
		}
	}
}
