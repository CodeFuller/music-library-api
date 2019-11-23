using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.Client.Queries;

namespace MusicLibraryApi.Client
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMusicLibraryApiClient(this IServiceCollection services)
		{
			services.AddTransient<IGenresQuery, GenresQuery>();
			services.AddTransient<IDiscsQuery, DiscsQuery>();

			return services;
		}
	}
}
