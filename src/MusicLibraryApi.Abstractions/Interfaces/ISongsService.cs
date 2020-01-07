using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface ISongsService
	{
		Task<int> CreateSong(Song song, Stream contentStream, CancellationToken cancellationToken);

		Task<int> CreateDeletedSong(Song song, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken);

		Task<IDictionary<int, Song>> GetSongs(IEnumerable<int> songIds, CancellationToken cancellationToken);

		Task<Song> GetSong(int songId, CancellationToken cancellationToken);

		Task<ILookup<int, Song>> GetSongsByDiscIds(IEnumerable<int> discIds, CancellationToken cancellationToken);

		Task<ILookup<int, Song>> GetSongsByArtistIds(IEnumerable<int> artistIds, CancellationToken cancellationToken);

		Task<ILookup<int, Song>> GetSongsByGenreIds(IEnumerable<int> genreIds, CancellationToken cancellationToken);
	}
}
