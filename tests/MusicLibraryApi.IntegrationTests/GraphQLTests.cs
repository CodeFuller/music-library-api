using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.IntegrationTests.Comparers;
using MusicLibraryApi.IntegrationTests.Comparers.Interfaces;

namespace MusicLibraryApi.IntegrationTests
{
	public abstract class GraphQLTests : IDisposable
	{
		protected CustomWebApplicationFactory WebApplicationFactory { get; } = new CustomWebApplicationFactory();

		protected IFolderDataComparer FoldersComparer { get; }

		protected IDiscDataComparer DiscsComparer { get; }

		protected IArtistDataComparer ArtistsComparer { get; }

		protected IGenreDataComparer GenresComparer { get; }

		protected ISongDataComparer SongsComparer { get; }

		protected GraphQLTests()
		{
			var songsComparer = new SongDataComparer();
			ArtistsComparer = new ArtistDataComparer(songsComparer);
			GenresComparer = new GenreDataComparer(songsComparer);
			var discsComparer = new DiscDataComparer(songsComparer);
			FoldersComparer = new FolderDataComparer(discsComparer);

			songsComparer.DiscsComparer = discsComparer;
			songsComparer.ArtistsComparer = ArtistsComparer;
			songsComparer.GenresComparer = GenresComparer;
			discsComparer.FoldersComparer = FoldersComparer;

			SongsComparer = songsComparer;
			DiscsComparer = discsComparer;
		}

		[TestInitialize]
		public void Initialize()
		{
			WebApplicationFactory.SeedData();
		}

		protected TClient CreateClient<TClient>()
		{
			return WebApplicationFactory.Services.GetRequiredService<TClient>();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				WebApplicationFactory?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
