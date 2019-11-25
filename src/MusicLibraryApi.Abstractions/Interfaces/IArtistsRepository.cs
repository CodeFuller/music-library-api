using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IArtistsRepository
	{
		Task<int> AddArtist(Artist artist, CancellationToken cancellationToken);

		Task<IEnumerable<Artist>> GetAllArtists(CancellationToken cancellationToken);
	}
}
