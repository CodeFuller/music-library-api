using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.Client.Operations;

namespace MusicLibraryApi.Client
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMusicLibraryApiClient(this IServiceCollection services)
		{
			services.AddTransient<IGenresQuery, GenreOperations>();
			services.AddTransient<IGenresMutation, GenreOperations>();
			services.AddTransient<IDiscsQuery, DiscOperations>();

			return services;
		}
	}
}
