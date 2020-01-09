using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Contracts.Statistics;
using MusicLibraryApi.IntegrationTests.DataCheckers;

namespace MusicLibraryApi.IntegrationTests
{
	public abstract class GraphQLTests : IDisposable
	{
		private CustomWebApplicationFactory? WebApplicationFactory { get; set; }

		private readonly FolderDataChecker foldersChecker;

		private readonly DiscDataChecker discsChecker;

		private readonly ArtistDataChecker artistsChecker;

		private readonly GenreDataChecker genresChecker;

		private readonly SongDataChecker songsChecker;

		private readonly PlaybackDataChecker playbacksChecker;

		private readonly StatisticsDataChecker statisticsChecker;

		private readonly SongsRatingsDataChecker songsRatingsChecker;

		protected GraphQLTests()
		{
			songsChecker = new SongDataChecker();
			artistsChecker = new ArtistDataChecker(songsChecker);
			genresChecker = new GenreDataChecker(songsChecker);
			discsChecker = new DiscDataChecker(songsChecker);
			foldersChecker = new FolderDataChecker(discsChecker);
			playbacksChecker = new PlaybackDataChecker(songsChecker);
			songsRatingsChecker = new SongsRatingsDataChecker();
			statisticsChecker = new StatisticsDataChecker(songsRatingsChecker);

			songsChecker.DiscsChecker = discsChecker;
			songsChecker.ArtistsChecker = artistsChecker;
			songsChecker.GenresChecker = genresChecker;
			songsChecker.PlaybacksChecker = playbacksChecker;
			discsChecker.FoldersChecker = foldersChecker;
		}

		[TestCleanup]
		public void Cleanup()
		{
			if (WebApplicationFactory != null)
			{
				WebApplicationFactory.CleanData();
				WebApplicationFactory = null;
			}
		}

		private CustomWebApplicationFactory InitializeWebApplicationFactory(Action<IServiceCollection> configureServices)
		{
			if (WebApplicationFactory == null)
			{
				WebApplicationFactory = new CustomWebApplicationFactory(configureServices);
				WebApplicationFactory.SeedData();
			}

			return WebApplicationFactory;
		}

		protected TClient CreateClient<TClient>(Action<IServiceCollection>? configureServices = null)
		{
			configureServices ??= services => { };
			return InitializeWebApplicationFactory(configureServices)
				.Services.GetRequiredService<TClient>();
		}

		protected HttpClient CreateClient()
		{
			return InitializeWebApplicationFactory(services => { })
				.CreateClient();
		}

		protected void AssertData(OutputFolderData? expected, OutputFolderData? actual)
		{
			foldersChecker.CheckData(expected, actual, String.Empty);
		}

		protected void AssertData(IEnumerable<OutputFolderData>? expected, IEnumerable<OutputFolderData>? actual)
		{
			foldersChecker.CheckData(expected?.ToList(), actual?.ToList(), String.Empty);
		}

		protected void AssertData(OutputDiscData? expected, OutputDiscData? actual)
		{
			discsChecker.CheckData(expected, actual, String.Empty);
		}

		protected void AssertData(IEnumerable<OutputDiscData>? expected, IEnumerable<OutputDiscData>? actual)
		{
			discsChecker.CheckData(expected?.ToList(), actual?.ToList(), String.Empty);
		}

		protected void AssertData(OutputArtistData? expected, OutputArtistData? actual)
		{
			artistsChecker.CheckData(expected, actual, String.Empty);
		}

		protected void AssertData(IEnumerable<OutputArtistData>? expected, IEnumerable<OutputArtistData>? actual)
		{
			artistsChecker.CheckData(expected?.ToList(), actual?.ToList(), String.Empty);
		}

		protected void AssertData(OutputGenreData? expected, OutputGenreData? actual)
		{
			genresChecker.CheckData(expected, actual, String.Empty);
		}

		protected void AssertData(IEnumerable<OutputGenreData>? expected, IEnumerable<OutputGenreData>? actual)
		{
			genresChecker.CheckData(expected?.ToList(), actual?.ToList(), String.Empty);
		}

		protected void AssertData(OutputSongData? expected, OutputSongData? actual)
		{
			songsChecker.CheckData(expected, actual, String.Empty);
		}

		protected void AssertData(IEnumerable<OutputSongData>? expected, IEnumerable<OutputSongData>? actual)
		{
			songsChecker.CheckData(expected?.ToList(), actual?.ToList(), String.Empty);
		}

		protected void AssertData(OutputPlaybackData? expected, OutputPlaybackData? actual)
		{
			playbacksChecker.CheckData(expected, actual, String.Empty);
		}

		protected void AssertData(IEnumerable<OutputPlaybackData>? expected, IEnumerable<OutputPlaybackData>? actual)
		{
			playbacksChecker.CheckData(expected?.ToList(), actual?.ToList(), String.Empty);
		}

		protected void AssertData(OutputStatisticsData? expected, OutputStatisticsData? actual)
		{
			statisticsChecker.CheckData(expected, actual, String.Empty);
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

		protected string GetFullContentPath(string relativeContentPath)
		{
			if (WebApplicationFactory == null)
			{
				throw new InvalidOperationException($"{nameof(WebApplicationFactory)} is not set");
			}

			if (WebApplicationFactory.FileSystemStorageRoot == null)
			{
				throw new InvalidOperationException($"{nameof(WebApplicationFactory.FileSystemStorageRoot)} is not set");
			}

			return Path.Combine(WebApplicationFactory.FileSystemStorageRoot, relativeContentPath);
		}

		protected void AssertSongContent(string relativeContentPath, byte[] expectedContent)
		{
			var fullContentPath = GetFullContentPath(relativeContentPath);

			var content = File.ReadAllBytes(fullContentPath);
			CollectionAssert.AreEqual(expectedContent, content);

			var fileInfo = new FileInfo(fullContentPath);
			Assert.IsTrue(fileInfo.IsReadOnly);
		}
	}
}
