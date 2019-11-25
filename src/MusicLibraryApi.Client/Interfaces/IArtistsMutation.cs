using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Artists;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IArtistsMutation
	{
		Task<int> CreateArtist(InputArtistData artistData, CancellationToken cancellationToken);
	}
}
