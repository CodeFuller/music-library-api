using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Logic.Interfaces;
using MusicLibraryApi.Logic.Internal;
using MusicLibraryApi.Logic.Services;
using MusicLibraryApi.Logic.Settings;

namespace MusicLibraryApi.Logic.Extensions
{
	public static class LogicServiceCollectionExtensions
	{
		public static IServiceCollection AddLogic(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ApiLogicSettings>(configuration.Bind);
			services.Configure<FileSystemStorageSettings>(options => configuration.Bind("fileSystemStorage", options));

			services.AddTransient<IGenresService, GenresService>();
			services.AddTransient<IArtistsService, ArtistsService>();
			services.AddTransient<IFoldersService, FoldersService>();
			services.AddTransient<IDiscsService, DiscsService>();
			services.AddTransient<ISongsService, SongsService>();
			services.AddTransient<IPlaybacksService, PlaybacksService>();
			services.AddTransient<IStatisticsService, StatisticsService>();
			services.AddTransient<IStorageService, StorageService>();

			services.AddSingleton<IChecksumCalculator, Crc32Calculator>();
			services.AddTransient<IContentStorage, FileSystemContentStorage>();

			return services;
		}
	}
}
