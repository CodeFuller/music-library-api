using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Interfaces
{
	public interface IContextServiceAccessor
	{
		IGenresService GenresService { get; }

		IArtistsService ArtistsService { get; }

		IFoldersService FoldersService { get; }

		IDiscsService DiscsService { get; }
	}
}
