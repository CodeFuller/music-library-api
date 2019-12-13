using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Playbacks;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IPlaybacksMutation
	{
		Task<int> AddSongPlayback(InputPlaybackData playbackData, CancellationToken cancellationToken);
	}
}
