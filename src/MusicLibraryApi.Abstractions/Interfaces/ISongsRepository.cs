using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface ISongsRepository
	{
		Task<int> CreateSong(int discId, int? artistId, int? genreId, Song song, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetDiscSongs(int discId, CancellationToken cancellationToken);
	}
}
