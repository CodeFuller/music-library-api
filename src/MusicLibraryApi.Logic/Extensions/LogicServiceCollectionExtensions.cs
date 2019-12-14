using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.Logic.Extensions
{
	public static class LogicServiceCollectionExtensions
	{
		public static IServiceCollection AddLogic(this IServiceCollection services)
		{
			services.AddTransient<IGenresService, GenresService>();
			services.AddTransient<IArtistsService, ArtistsService>();
			services.AddTransient<IFoldersService, FoldersService>();
			services.AddTransient<IDiscsService, DiscsService>();
			services.AddTransient<ISongsService, SongsService>();
			services.AddTransient<IPlaybacksService, PlaybacksService>();
			services.AddTransient<IStatisticsService, StatisticsService>();

			return services;
		}
	}
}
