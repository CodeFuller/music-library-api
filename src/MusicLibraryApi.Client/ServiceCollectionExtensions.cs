using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

			services.AddHttpClient(BasicQuery.HttpClientName, (serviceProvider, httpClient) =>
			{
				var options = serviceProvider.GetRequiredService<IOptions<ApiConnectionSettings>>();
				var apiUri = options?.Value?.BaseUrl;
				if (apiUri == null)
				{
					throw new InvalidOperationException("MusicLibrary API base URL (MusicLibraryApiBaseUrl) is not defined");
				}

				httpClient.BaseAddress = apiUri;
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			});

			return services;
		}
	}
}
