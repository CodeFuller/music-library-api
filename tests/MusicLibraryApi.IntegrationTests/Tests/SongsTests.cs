using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.IntegrationTests.Comparers;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class SongsTests : GraphQLTests
	{
		[TestMethod]
		public async Task SongsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedSongs = new[]
			{
				new OutputSongData(1, "Hell's Bells", "02 - Hell's Bells.mp3", 2, new TimeSpan(0, 5, 12),
					Rating.R4, 320000, new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), 4),

				new OutputSongData(2, "Highway To Hell", "01 - Highway To Hell.mp3", 1, new TimeSpan(0, 3, 28),
					Rating.R6, 320000, new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), 4),

				new OutputSongData(3, "Are You Ready?", "03 - Are You Ready?.mp3", null, new TimeSpan(0, 4, 09), null, null, null, 0),
			};

			var client = CreateClient<ISongsQuery>();

			// Act

			var receivedSongs = await client.GetSongs(SongFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), new SongDataComparer());
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

			// Checking new songs data

			var expectedSongs = new[]
			{
				new OutputSongData(1, "Hell's Bells", "02 - Hell's Bells.mp3", 2, new TimeSpan(0, 5, 12),
					Rating.R4, 320000, new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), 4),

				new OutputSongData(2, "Highway To Hell", "01 - Highway To Hell.mp3", 1, new TimeSpan(0, 3, 28),
					Rating.R6, 320000, new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), 4),

				new OutputSongData(3, "Are You Ready?", "03 - Are You Ready?.mp3", null, new TimeSpan(0, 4, 09), null, null, null, 0),

				new OutputSongData(4, "Hail Caesar", "04 - Hail Caesar.mp3", 4, new TimeSpan(0, 5, 13), Rating.R4, 320000,
					new DateTimeOffset(2018, 11, 25, 08, 35, 28, TimeSpan.FromHours(2)), 4),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), new SongDataComparer());
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

			// Checking new songs data

			var expectedSongs = new[]
			{
				new OutputSongData(1, "Hell's Bells", "02 - Hell's Bells.mp3", 2, new TimeSpan(0, 5, 12),
					Rating.R4, 320000, new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), 4),

				new OutputSongData(2, "Highway To Hell", "01 - Highway To Hell.mp3", 1, new TimeSpan(0, 3, 28),
					Rating.R6, 320000, new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), 4),

				new OutputSongData(3, "Are You Ready?", "03 - Are You Ready?.mp3", null, new TimeSpan(0, 4, 09), null, null, null, 0),

				new OutputSongData(4, "Hail Caesar", "04 - Hail Caesar.mp3", null, new TimeSpan(0, 5, 13), null, null, null, 0),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), new SongDataComparer());
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
				new OutputSongData(1, null, null, null, null, null, null, null, null),
				new OutputSongData(2, null, null, null, null, null, null, null, null),
				new OutputSongData(3, null, null, null, null, null, null, null, null),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), new SongDataComparer());
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
				new OutputSongData(1, null, null, null, null, null, null, null, null),
				new OutputSongData(2, null, null, null, null, null, null, null, null),
				new OutputSongData(3, null, null, null, null, null, null, null, null),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), new SongDataComparer());
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
				new OutputSongData(1, null, null, null, null, null, null, null, null),
				new OutputSongData(2, null, null, null, null, null, null, null, null),
				new OutputSongData(3, null, null, null, null, null, null, null, null),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedSongs, receivedSongs.ToList(), new SongDataComparer());
		}
	}
}
