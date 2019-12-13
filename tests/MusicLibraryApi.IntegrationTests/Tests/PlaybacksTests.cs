using System;
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
				new OutputPlaybackData(id: 3, playbackTime: new DateTimeOffset(2015, 10, 23, 15, 18, 43, TimeSpan.FromHours(2)), song: new OutputSongData(id: 1)),
				new OutputPlaybackData(id: 2, playbackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), song: new OutputSongData(id: 2)),
				new OutputPlaybackData(id: 1, playbackTime: new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), song: new OutputSongData(id: 1)),
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

			var expectedPlayback = new OutputPlaybackData(id: 2, playbackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), song: new OutputSongData(id: 2));

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

			var newPlaybackData = new InputPlaybackData(3, new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)));

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var newSongId = await client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			Assert.AreEqual(4, newSongId);

			// Checking created playback data

			var expectedSong = new OutputSongData(lastPlaybackTime: new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), playbacksCount: 1,
				playbacks: new[] { new OutputPlaybackData(id: 4, playbackTime: new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2))), });

			var songsQuery = CreateClient<ISongsQuery>();
			var requestedFields = SongFields.PlaybacksCount + SongFields.LastPlaybackTime + SongFields.PlaybacksCount + SongFields.Playbacks(PlaybackFields.Id + PlaybackFields.PlaybackTime);
			var receivedSong = await songsQuery.GetSong(3, requestedFields, CancellationToken.None);

			AssertData(expectedSong, receivedSong);
		}

		[TestMethod]
		public async Task CreatePlaybackMutation_ForSongWithExistingPlaybacks_UpdatesSongPlaybacks()
		{
			// Arrange

			var newPlaybackData = new InputPlaybackData(1, new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)));

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var newSongId = await client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			Assert.AreEqual(4, newSongId);

			// Checking created playback data

			var expectedPlaybacks = new[]
			{
				new OutputPlaybackData(id: 3, playbackTime: new DateTimeOffset(2015, 10, 23, 15, 18, 43, TimeSpan.FromHours(2))),
				new OutputPlaybackData(id: 1, playbackTime: new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2))),
				new OutputPlaybackData(id: 4, playbackTime: new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2))),
			};

			var expectedSong = new OutputSongData(lastPlaybackTime: new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)), playbacksCount: 3, playbacks: expectedPlaybacks);

			var songsQuery = CreateClient<ISongsQuery>();
			var requestedFields = SongFields.PlaybacksCount + SongFields.LastPlaybackTime + SongFields.PlaybacksCount + SongFields.Playbacks(PlaybackFields.Id + PlaybackFields.PlaybackTime);
			var receivedSong = await songsQuery.GetSong(1, requestedFields, CancellationToken.None);

			AssertData(expectedSong, receivedSong);
		}

		[TestMethod]
		public async Task CreatePlaybackMutation_ForUnknownSong_ReturnsError()
		{
			// Arrange

			var newPlaybackData = new InputPlaybackData(12345, new DateTimeOffset(2019, 12, 13, 07, 05, 42, TimeSpan.FromHours(2)));

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

			var newPlaybackData = new InputPlaybackData(1, new DateTimeOffset(2018, 11, 25, 08, 18, 17, TimeSpan.FromHours(2)));

			var client = CreateClient<IPlaybacksMutation>();

			// Act

			var addPlaybackTask = client.AddSongPlayback(newPlaybackData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => addPlaybackTask);
			Assert.AreEqual("Can not add earlier playback for the song: 2018.11.25 09:18:17 <= 2018.11.25 09:25:17", exception.Message);
		}
	}
}
