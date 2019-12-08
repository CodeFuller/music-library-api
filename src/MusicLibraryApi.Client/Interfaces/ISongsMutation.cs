using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface ISongsMutation
	{
		Task<int> CreateSong(InputSongData songData, CancellationToken cancellationToken);
	}
}
