using System;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.Internal
{
	public class ServiceAccessor : IServiceAccessor
	{
		private readonly IServiceProvider serviceProvider;

		public IGenresService GenresService => GetService<IGenresService>();

		public IArtistsService ArtistsService => GetService<IArtistsService>();

		public IFoldersService FoldersService => GetService<IFoldersService>();

		public IDiscsService DiscsService => GetService<IDiscsService>();

		public ISongsService SongsService => GetService<ISongsService>();

		public IPlaybacksService PlaybacksService => GetService<IPlaybacksService>();

		public ServiceAccessor(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private TService GetService<TService>()
		{
			return serviceProvider.GetRequiredService<TService>();
		}
	}
}
