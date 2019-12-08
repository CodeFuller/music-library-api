using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface ISongsQuery
	{
		Task<IReadOnlyCollection<OutputSongData>> GetSongs(QueryFieldSet<SongQuery> fields, CancellationToken cancellationToken);
	}
}
