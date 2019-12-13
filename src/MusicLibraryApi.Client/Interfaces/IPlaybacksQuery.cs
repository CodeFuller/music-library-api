using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Fields;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IPlaybacksQuery
	{
		Task<IReadOnlyCollection<OutputPlaybackData>> GetPlaybacks(QueryFieldSet<OutputPlaybackData> fields, CancellationToken cancellationToken);

		Task<OutputPlaybackData> GetPlayback(int playbackId, QueryFieldSet<OutputPlaybackData> fields, CancellationToken cancellationToken);
	}
}
