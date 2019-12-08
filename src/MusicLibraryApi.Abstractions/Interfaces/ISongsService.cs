using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface ISongsService
	{
		Task<int> CreateSong(int discId, int? artistId, int? genreId, Song song, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken);

		Task<Song> GetSong(int songId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetDiscSongs(int discId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetGenreSongs(int genreId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetArtistSongs(int artistId, CancellationToken cancellationToken);
	}
}
