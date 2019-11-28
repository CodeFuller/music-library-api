using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Interfaces
{
	public interface IContextServiceAccessor
	{
		IGenresRepository GenresRepository { get; }

		IArtistsRepository ArtistsRepository { get; }

		IFoldersService FoldersService { get; }

		IDiscsRepository DiscsRepository { get; }
	}
}
