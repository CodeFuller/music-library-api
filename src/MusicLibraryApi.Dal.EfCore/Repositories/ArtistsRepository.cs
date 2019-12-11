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
	public class ArtistsRepository : IArtistsRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public ArtistsRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreateArtist(Artist artist, CancellationToken cancellationToken)
		{
			var artistEntity = mapper.Map<ArtistEntity>(artist);

			context.Artists.Add(artistEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsUniqueViolationException())
			{
				throw new DuplicateKeyException($"Failed to add artist '{artist.Name}' to the database", e);
			}

			return artistEntity.Id;
		}

		public async Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken)
		{
			return await context.Artists
				.Select(a => mapper.Map<Artist>(a))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Artist>> GetArtists(IEnumerable<int> artistIds, CancellationToken cancellationToken)
		{
			return await context.Artists.Where(a => artistIds.Contains(a.Id))
				.Select(a => mapper.Map<Artist>(a))
				.ToListAsync(cancellationToken);
		}

		public async Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken)
		{
			var artistEntity = await context.Artists
				.Where(x => x.Id == artistId)
				.SingleOrDefaultAsync(cancellationToken);

			if (artistEntity == null)
			{
				throw new ArtistNotFoundException(Invariant($"The artist with id of {artistId} does not exist"));
			}

			return mapper.Map<Artist>(artistEntity);
		}
	}
}
