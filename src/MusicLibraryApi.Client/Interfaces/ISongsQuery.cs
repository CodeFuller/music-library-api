using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Fields;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface ISongsQuery
	{
		Task<IReadOnlyCollection<OutputSongData>> GetSongs(QueryFieldSet<OutputSongData> fields, CancellationToken cancellationToken);

		Task<OutputSongData> GetSong(int songId, QueryFieldSet<OutputSongData> fields, CancellationToken cancellationToken);
	}
}
