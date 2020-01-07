using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface ISongsMutation
	{
		/// <summary>
		/// Creates active song with a content.
		/// </summary>
		/// <param name="songData">Input song data.</param>
		/// <param name="songContent">Song content.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Id of created song.</returns>
		Task<int> CreateSong(InputSongData songData, Stream songContent, CancellationToken cancellationToken);

		/// <summary>
		/// Creates deleted song without a content.
		/// </summary>
		/// <param name="songData">Input song data.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Id of created song.</returns>
		Task<int> CreateDeletedSong(InputSongData songData, CancellationToken cancellationToken);
	}
}
