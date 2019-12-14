using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Interfaces
{
	public interface IServiceAccessor
	{
		IGenresService GenresService { get; }

		IArtistsService ArtistsService { get; }

		IFoldersService FoldersService { get; }

		IDiscsService DiscsService { get; }

		ISongsService SongsService { get; }

		IPlaybacksService PlaybacksService { get; }

		IStatisticsService StatisticsService { get; }
	}
}
