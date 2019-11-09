using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Interfaces
{
	public interface IContextRepositoryAccessor
	{
		IGenresRepository GenresRepository { get; }

		IDiscsRepository DiscsRepository { get; }

		ISongsRepository SongsRepository { get; }
	}
}
