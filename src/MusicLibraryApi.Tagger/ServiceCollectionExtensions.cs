using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Tagger
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddTagger(this IServiceCollection services)
		{
			services.AddTransient<ISongTagger, SongTagger>();

			return services;
		}
	}
}
