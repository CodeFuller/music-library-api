﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class PlaybacksTests : GraphQLTests
	{
		private static QueryFieldSet<OutputPlaybackData> RequestedFields => PlaybackFields.All + PlaybackFields.Song(SongFields.Id);

		[TestMethod]
		public async Task PlaybacksQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedPlaybacks = new[]
			{
				new OutputPlaybackData
				{
					Id = 3,
					PlaybackTime = new DateTimeOffset(2015, 10, 23, 15, 18, 43, TimeSpan.FromHours(2)),
					Song = new OutputSongData { Id = 1, },
				},

				new OutputPlaybackData
				{
					Id = 2,
					PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
					Song = new OutputSongData { Id = 2, },
				},

				new OutputPlaybackData
				{
					Id = 1,
					PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)),
					Song = new OutputSongData { Id = 1, },
				},

				new OutputPlaybackData
				{
					Id = 4,
					PlaybackTime = new DateTimeOffset(2019, 12, 14, 17, 27, 04, TimeSpan.FromHours(2)),
					Song = new OutputSongData { Id = 4, },
				},
			};

			var client = CreateClient<IPlaybacksQuery>();

			// Act

			var receivedPlaybacks = await client.GetPlaybacks(RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedPlaybacks, receivedPlaybacks);
		}

		[TestMethod]
		public async Task PlaybackQuery_ForExistingPlayback_ReturnsCorrectData()
		{
			// Arrange

			var expectedPlayback = new OutputPlaybackData
			{
				Id = 2,
				PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
				Song = new OutputSongData { Id = 2, },
			};

			var client = CreateClient<IPlaybacksQuery>();

			// Act

			var receivedPlayback = await client.GetPlayback(2, RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedPlayback, receivedPlayback);
		}

		[TestMethod]
		public async Task PlaybackQuery_ForUnknownPlayback_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IPlaybacksQuery>();

			// Act

			var getSongTask = client.GetPlayback(12345, RequestedFields, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getSongTask);
			Assert.AreEqual("The playback with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreatePlaybackMutation_ForSongWithoutPlaybacks_UpdatesSongPlaybacks()
		{
			// Arrange

			var newPlaybackData = new InputPlaybackData { SongId = 3, PlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), };

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var newSongId = await client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newSongId);

			// Checking created playback data

			var expectedSong = new OutputSongData
			{
				LastPlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)),
				PlaybacksCount = 1,
				Playbacks = new[]
				{
					new OutputPlaybackData { Id = 5, PlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), },
				},
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var requestedFields = SongFields.PlaybacksCount + SongFields.LastPlaybackTime + SongFields.PlaybacksCount + SongFields.Playbacks(PlaybackFields.Id + PlaybackFields.PlaybackTime);
			var receivedSong = await songsQuery.GetSong(3, requestedFields, CancellationToken.None);

			AssertData(expectedSong, receivedSong);
		}

		[TestMethod]
		public async Task CreatePlaybackMutation_ForSongWithExistingPlaybacks_UpdatesSongPlaybacks()
		{
			// Arrange

			var newPlaybackData = new InputPlaybackData { SongId = 1, PlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), };

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var newSongId = await client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newSongId);

			// Checking created playback data

			var expectedSong = new OutputSongData
			{
				LastPlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)),
				PlaybacksCount = 3,
				Playbacks = new[]
				{
					new OutputPlaybackData { Id = 3, PlaybackTime = new DateTimeOffset(2015, 10, 23, 15, 18, 43, TimeSpan.FromHours(2)), },
					new OutputPlaybackData { Id = 1, PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), },
					new OutputPlaybackData { Id = 5, PlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), },
				},
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var requestedFields = SongFields.PlaybacksCount + SongFields.LastPlaybackTime + SongFields.PlaybacksCount + SongFields.Playbacks(PlaybackFields.Id + PlaybackFields.PlaybackTime);
			var receivedSong = await songsQuery.GetSong(1, requestedFields, CancellationToken.None);

			AssertData(expectedSong, receivedSong);
		}

		[TestMethod]
		public async Task CreatePlaybackMutation_ForUnknownSong_ReturnsError()
		{
			// Arrange

			var newPlaybackData = new InputPlaybackData { SongId = 12345, PlaybackTime = new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), };

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var addPlaybackTask = client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => addPlaybackTask);
			Assert.AreEqual("The song with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreatePlaybackMutation_ForInconsistentPlaybackTime_ReturnsError()
		{
			// Arrange

			var newPlaybackData = new InputPlaybackData { SongId = 1, PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 18, 17, TimeSpan.FromHours(2)), };

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var addPlaybackTask = client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => addPlaybackTask);
			Assert.AreEqual("Can not add earlier playback for the song: 2018.11.25 06:18:17 +00 <= 2018.11.25 06:25:17 +00", exception.Message);
		}
	}
}
