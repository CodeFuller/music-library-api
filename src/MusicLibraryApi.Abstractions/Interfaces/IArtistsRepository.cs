﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IArtistsRepository
	{
		Task AddArtist(Artist artist, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Artist>> GetArtists(IEnumerable<int> artistIds, CancellationToken cancellationToken);

		Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken);
	}
}
