using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface ISongsRepository
	{
		Task<int> CreateSong(Song song, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetSongs(IEnumerable<int> songIds, CancellationToken cancellationToken);

		Task<Song> GetSong(int songId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetSongsByDiscIds(IEnumerable<int> discIds, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetSongsByArtistIds(IEnumerable<int> artistIds, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetSongsByGenreIds(IEnumerable<int> genreIds, CancellationToken cancellationToken);
	}
}
