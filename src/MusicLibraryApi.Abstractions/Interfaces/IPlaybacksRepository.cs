using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IPlaybacksRepository
	{
		Task<int> CreatePlayback(Playback playback, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Playback>> GetAllPlaybacks(CancellationToken cancellationToken);

		Task<Playback> GetPlayback(int playbackId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Playback>> GetPlaybacksBySongIds(IEnumerable<int> songIds, CancellationToken cancellationToken);
	}
}
