using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.IntegrationTests.DataCheckers;

namespace MusicLibraryApi.IntegrationTests
{
	public abstract class GraphQLTests : IDisposable
	{
		protected CustomWebApplicationFactory WebApplicationFactory { get; } = new CustomWebApplicationFactory();

		private readonly FolderDataChecker foldersChecker;

		private readonly DiscDataChecker discsChecker;

		private readonly ArtistDataChecker artistsChecker;

		private readonly GenreDataChecker genresChecker;

		private readonly SongDataChecker songsChecker;

		protected GraphQLTests()
		{
			songsChecker = new SongDataChecker();
			artistsChecker = new ArtistDataChecker(songsChecker);
			genresChecker = new GenreDataChecker(songsChecker);
			discsChecker = new DiscDataChecker(songsChecker);
			foldersChecker = new FolderDataChecker(discsChecker);

			songsChecker.DiscsChecker = discsChecker;
			songsChecker.ArtistsChecker = artistsChecker;
			songsChecker.GenresChecker = genresChecker;
			discsChecker.FoldersChecker = foldersChecker;
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
