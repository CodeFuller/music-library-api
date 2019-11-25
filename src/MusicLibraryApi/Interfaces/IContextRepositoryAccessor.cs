using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Interfaces
{
	public interface IContextRepositoryAccessor
	{
		IGenresRepository GenresRepository { get; }

		IArtistsRepository ArtistsRepository { get; }

		IDiscsRepository DiscsRepository { get; }
	}
}
