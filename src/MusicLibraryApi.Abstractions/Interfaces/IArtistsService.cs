using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IArtistsService
	{
		Task<int> CreateArtist(Artist artist, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken);

		Task<IDictionary<int, Artist>> GetArtists(IEnumerable<int> artistIds, CancellationToken cancellationToken);

		Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken);
	}
}
