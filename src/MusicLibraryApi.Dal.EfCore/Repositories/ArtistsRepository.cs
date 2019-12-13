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
	public class ArtistsRepository : IArtistsRepository
	{
		private readonly MusicLibraryDbContext context;

		public ArtistsRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Task AddArtist(Artist artist, CancellationToken cancellationToken)
		{
			context.Artists.Add(artist);
			return Task.CompletedTask;
		}

		public async Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken)
		{
			return await context.Artists
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Artist>> GetArtists(IEnumerable<int> artistIds, CancellationToken cancellationToken)
		{
			return await context.Artists.Where(a => artistIds.Contains(a.Id))
				.ToListAsync(cancellationToken);
		}

		public async Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken)
		{
			var artist = await context.Artists
				.Where(a => a.Id == artistId)
				.SingleOrDefaultAsync(cancellationToken);

			if (artist == null)
			{
				throw new ArtistNotFoundException(Invariant($"The artist with id of {artistId} does not exist"));
			}

			return artist;
		}
	}
}
