using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.Logic
{
	public static class LogicServiceCollectionExtensions
	{
		public static IServiceCollection AddLogic(this IServiceCollection services)
		{
			services.AddTransient<IGenresService, GenresService>();
			services.AddTransient<IArtistsService, ArtistsService>();
			services.AddTransient<IFoldersService, FoldersService>();
			services.AddTransient<IDiscsService, DiscsService>();

			return services;
		}
	}
}
