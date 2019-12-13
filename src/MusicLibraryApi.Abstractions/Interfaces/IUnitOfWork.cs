using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IUnitOfWork
	{
		IFoldersRepository FoldersRepository { get; }

		IDiscsRepository DiscsRepository { get; }

		IArtistsRepository ArtistsRepository { get; }

		IGenresRepository GenresRepository { get; }

		ISongsRepository SongsRepository { get; }

		IPlaybacksRepository PlaybacksRepository { get; }

		Task Commit(CancellationToken cancellationToken);
	}
}
