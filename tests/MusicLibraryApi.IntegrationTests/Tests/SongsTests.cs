using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class SongsTests : GraphQLTests
	{
		private static QueryFieldSet<SongQuery> RequestedFields => SongFields.All + SongFields.Disc(DiscFields.Id) + SongFields.Artist(ArtistFields.Id) + SongFields.Genre(GenreFields.Id);

		[TestMethod]
		public async Task SongsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedSongs = new[]
			{
				new OutputSongData(id: 1, title: "Hell's Bells", treeTitle: "02 - Hell's Bells.mp3", trackNumber: 2, duration: new TimeSpan(0, 5, 12),
					disc: new OutputDiscData(id: 1), artist: null, genre: new OutputGenreData(id: 2), rating: Rating.R4, bitRate: 320000,
					lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), playbacksCount: 4),

				new OutputSongData(id: 2, title: "Highway To Hell", treeTitle: "01 - Highway To Hell.mp3", trackNumber: 1, duration: new TimeSpan(0, 3, 28),
					disc: new OutputDiscData(id: 1), artist: new OutputArtistData(id: 2), genre: new OutputGenreData(id: 1), rating: Rating.R6, bitRate: 320000,
					lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), playbacksCount: 4),

				new OutputSongData(id: 3, title: "Are You Ready?", treeTitle: "03 - Are You Ready?.mp3", duration: new TimeSpan(0, 4, 09),
					disc: new OutputDiscData(id: 1), artist: new OutputArtistData(id: 1), genre: null, playbacksCount: 0),
			};

			var client = CreateClient<ISongsQuery>();

			// Act

			var receivedSongs = await client.GetSongs(RequestedFields, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), SongsComparer);
		}

		[TestMethod]
		public async Task SongQuery_ForExistingSong_ReturnsCorrectData()
		{
			// Arrange

			var expectedData = new OutputSongData(id: 2, title: "Highway To Hell", treeTitle: "01 - Highway To Hell.mp3", trackNumber: 1, duration: new TimeSpan(0, 3, 28),
				disc: new OutputDiscData(id: 1), artist: new OutputArtistData(id: 2), genre: new OutputGenreData(id: 1), rating: Rating.R6, bitRate: 320000,
				lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), playbacksCount: 4);

			var client = CreateClient<ISongsQuery>();

			// Act

			var receivedData = await client.GetSong(2, RequestedFields, CancellationToken.None);

			// Assert

			var cmp = SongsComparer.Compare(expectedData, receivedData);
			Assert.AreEqual(0, cmp, "Songs data does not match");
		}

		[TestMethod]
		public async Task SongQuery_IfSongDoesNotExist_ReturnsError()
		{
			// Arrange

			var client = CreateClient<ISongsQuery>();

			// Act

			var getSongTask = client.GetSong(12345, RequestedFields, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getSongTask);
			Assert.AreEqual("The song with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreateSongMutation_OptionalDataFilled_CreatesSongSuccessfully()
		{
			// Arrange

			var newSongData = new InputSongData(1, 2, 3, "Hail Caesar", "04 - Hail Caesar.mp3", 4, new TimeSpan(0, 5, 13), Rating.R4, 320000,
				new DateTimeOffset(2018, 11, 25, 08, 35, 28, TimeSpan.FromHours(2)), 4);

			var client = CreateClient<ISongsMutation>();

			// Act

			var newSongId = await client.CreateSong(newSongData, CancellationToken.None);

			// Assert

			Assert.AreEqual(4, newSongId);

			// Checking created song data

			var expectedData = new OutputSongData(id: 4, title: "Hail Caesar", treeTitle: "04 - Hail Caesar.mp3", trackNumber: 4, duration: new TimeSpan(0, 5, 13),
				disc: new OutputDiscData(id: 1), artist: new OutputArtistData(id: 2), genre: new OutputGenreData(id: 3), rating: Rating.R4, bitRate: 320000,
				lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 35, 28, TimeSpan.FromHours(2)), playbacksCount: 4);

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedData = await songsQuery.GetSong(4, RequestedFields, CancellationToken.None);

			var cmp = SongsComparer.Compare(expectedData, receivedData);
			Assert.AreEqual(0, cmp, "Songs data does not match");
		}

		[TestMethod]
		public async Task CreateSongMutation_OptionalDataMissing_CreatesSongSuccessfully()
		{
			// Arrange

			var newSongData = new InputSongData(1, null, null, "Hail Caesar", "04 - Hail Caesar.mp3", null, new TimeSpan(0, 5, 13), null, null);

			var client = CreateClient<ISongsMutation>();

			// Act

			var newSongId = await client.CreateSong(newSongData, CancellationToken.None);

			// Assert

			Assert.AreEqual(4, newSongId);

			// Checking created song data

			var expectedData = new OutputSongData(id: 4, title: "Hail Caesar", treeTitle: "04 - Hail Caesar.mp3", trackNumber: null, duration: new TimeSpan(0, 5, 13),
				disc: new OutputDiscData(id: 1), artist: null, genre: null, rating: null, bitRate: null,
				lastPlaybackTime: null, playbacksCount: 0);

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedData = await songsQuery.GetSong(4, RequestedFields, CancellationToken.None);

			var cmp = SongsComparer.Compare(expectedData, receivedData);
			Assert.AreEqual(0, cmp, "Songs data does not match");
		}

		[TestMethod]
		public async Task CreateSongMutation_ForUnknownDisc_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData(12345, null, null, "Hail Caesar", "04 - Hail Caesar.mp3", null, new TimeSpan(0, 5, 13), null, null);

			var client = CreateClient<ISongsMutation>();

			// Act

			var createSongTask = client.CreateSong(newSongData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("The disc with id of '12345' does not exist", exception.Message);

			// Checking that no song was created

			var expectedSongs = new[]
			{
				new OutputSongData(id: 1),
				new OutputSongData(id: 2),
				new OutputSongData(id: 3),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), SongsComparer);
		}

		[TestMethod]
		public async Task CreateSongMutation_ForUnknownArtist_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData(1, 12345, null, "Hail Caesar", "04 - Hail Caesar.mp3", null, new TimeSpan(0, 5, 13), null, null);

			var client = CreateClient<ISongsMutation>();

			// Act

			var createSongTask = client.CreateSong(newSongData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("The artist with id of '12345' does not exist", exception.Message);

			// Checking that no song was created

			var expectedSongs = new[]
			{
				new OutputSongData(id: 1),
				new OutputSongData(id: 2),
				new OutputSongData(id: 3),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), SongsComparer);
		}

		[TestMethod]
		public async Task CreateSongMutation_ForUnknownGenre_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData(1, null, 12345, "Hail Caesar", "04 - Hail Caesar.mp3", null, new TimeSpan(0, 5, 13), null, null);

			var client = CreateClient<ISongsMutation>();

			// Act

			var createSongTask = client.CreateSong(newSongData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("The genre with id of '12345' does not exist", exception.Message);

			// Checking that no song was created

			var expectedSongs = new[]
			{
				new OutputSongData(id: 1),
				new OutputSongData(id: 2),
				new OutputSongData(id: 3),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), SongsComparer);
		}
	}
}
