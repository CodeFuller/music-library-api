using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Extensions;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Services
{
	public class ArtistsService : IArtistsService
	{
		private readonly IArtistsRepository repository;

		private readonly ILogger<ArtistsService> logger;

		public ArtistsService(IArtistsRepository repository, ILogger<ArtistsService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> CreateArtist(Artist artist, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.CreateArtist(artist, cancellationToken);
			}
			catch (DuplicateKeyException e)
			{
				logger.LogError(e, "Artist {ArtistName} already exists", artist.Name);
				throw new ServiceOperationFailedException(Invariant($"Artist '{artist.Name}' already exists"), e);
			}
		}

		public async Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken)
		{
			var artists = await repository.GetAllArtists(cancellationToken);
			return artists.OrderBy(a => a.Name).ToList();
		}

		public async Task<IDictionary<int, Artist>> GetArtists(IEnumerable<int> artistIds, CancellationToken cancellationToken)
		{
			var artists = await repository.GetArtists(artistIds, cancellationToken);
			return artists.ToDictionary(a => a.Id);
		}

		public async Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetArtist(artistId, cancellationToken);
			}
			catch (ArtistNotFoundException e)
			{
				throw e.Handle(artistId, logger);
			}
		}
	}
}
