using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Interfaces
{
	public interface IContextRepositoryAccessor
	{
		IDiscsRepository DiscsRepository { get; }

		ISongsRepository SongsRepository { get; }
	}
}
