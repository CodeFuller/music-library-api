﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.IntegrationTests.Utility;
using MusicLibraryApi.Logic.Interfaces;
using Rating = MusicLibraryApi.Client.Contracts.Rating;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class SongsTests : GraphQLTests
	{
		private static QueryFieldSet<OutputSongData> RequestedFields => SongFields.All + SongFields.Disc(DiscFields.Id) + SongFields.Artist(ArtistFields.Id) +
		                                                                SongFields.Genre(GenreFields.Id) + SongFields.Playbacks(PlaybackFields.Id);

		[TestMethod]
		public async Task SongsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedSongs = new[]
			{
				new OutputSongData
				{
					Id = 1,
					Title = "Hell's Bells",
					TreeTitle = "02 - Hell's Bells.mp3",
					TrackNumber = 2,
					Duration = new TimeSpan(0, 5, 12),
					Disc = new OutputDiscData { Id = 1, },
					Genre = new OutputGenreData { Id = 2, },
					Rating = Rating.R4,
					BitRate = 320000,
					Size = 1243,
					LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)),
					PlaybacksCount = 2,
					Playbacks = new[] { new OutputPlaybackData { Id = 3, }, new OutputPlaybackData { Id = 1, }, },
				},

				new OutputSongData
				{
					Id = 2,
					Title = "Highway To Hell",
					TreeTitle = "01 - Highway To Hell.mp3",
					TrackNumber = 1,
					Duration = new TimeSpan(0, 3, 28),
					Disc = new OutputDiscData { Id = 1, },
					Artist = new OutputArtistData { Id = 2, },
					Genre = new OutputGenreData { Id = 1, },
					Rating = Rating.R6,
					BitRate = 320000,
					Size = 946,
					LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
					PlaybacksCount = 1,
					Playbacks = new[] { new OutputPlaybackData { Id = 2, } },
				},

				new OutputSongData
				{
					Id = 3,
					Title = "Are You Ready?",
					TreeTitle = "03 - Are You Ready?.mp3",
					Duration = new TimeSpan(0, 4, 09),
					Disc = new OutputDiscData { Id = 2, },
					Artist = new OutputArtistData { Id = 1, },
					Size = 673,
					PlaybacksCount = 0,
					Playbacks = Array.Empty<OutputPlaybackData>(),
				},
			};

			var client = CreateClient<ISongsQuery>();

			// Act

			var receivedSongs = await client.GetSongs(RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedSongs, receivedSongs);
		}

		[TestMethod]
		public async Task SongQuery_ForExistingSong_ReturnsCorrectData()
		{
			// Arrange

			var expectedSong = new OutputSongData
			{
				Id = 2,
				Title = "Highway To Hell",
				TreeTitle = "01 - Highway To Hell.mp3",
				TrackNumber = 1,
				Duration = new TimeSpan(0, 3, 28),
				Disc = new OutputDiscData { Id = 1, },
				Artist = new OutputArtistData { Id = 2, },
				Genre = new OutputGenreData { Id = 1, },
				Rating = Rating.R6,
				BitRate = 320000,
				Size = 946,
				LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
				PlaybacksCount = 1,
				Playbacks = new[] { new OutputPlaybackData { Id = 2, } },
			};

			var client = CreateClient<ISongsQuery>();

			// Act

			var receivedSong = await client.GetSong(2, RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedSong, receivedSong);
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
		public async Task CreateSongMutation_ActiveSongWithOptionalDataFilled_CreatesSongSuccessfully()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				ArtistId = 2,
				GenreId = 3,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				TrackNumber = 4,
				Duration = new TimeSpan(0, 5, 13),
				Rating = Rating.R4,
				BitRate = 320000,
			};

			var client = CreateClient<ISongsMutation>();

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));

			// Act

			var newSongId = await client.CreateSong(newSongData, songContent, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newSongId);

			// Checking created song data

			var expectedSong = new OutputSongData
			{
				Id = 5,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				TrackNumber = 4,
				Duration = new TimeSpan(0, 5, 13),
				Disc = new OutputDiscData { Id = 1, },
				Artist = new OutputArtistData { Id = 2 },
				Genre = new OutputGenreData { Id = 3, },
				Rating = Rating.R4,
				BitRate = 320000,
				Size = 405586,
				LastPlaybackTime = null,
				PlaybacksCount = 0,
				Playbacks = Array.Empty<OutputPlaybackData>(),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSong = await songsQuery.GetSong(5, RequestedFields, CancellationToken.None);

			var expectedContent = await File.ReadAllBytesAsync(Paths.GetTestDataFilePath("Output Song With Optional Data Filled.mp3"));

			AssertData(expectedSong, receivedSong);
			AssertStorageContent("2001 - Platinum Hits (CD 2)/04 - Hail Caesar.mp3", expectedContent);
		}

		[TestMethod]
		public async Task CreateSongMutation_ActiveSongWithOptionalDataMissing_CreatesSongSuccessfully()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var client = CreateClient<ISongsMutation>();

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));

			// Act

			var newSongId = await client.CreateSong(newSongData, songContent, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newSongId);

			// Checking created song data

			var expectedSong = new OutputSongData
			{
				Id = 5,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
				Size = 405503,
				Disc = new OutputDiscData { Id = 1, },
				PlaybacksCount = 0,
				Playbacks = Array.Empty<OutputPlaybackData>(),
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSong = await songsQuery.GetSong(5, RequestedFields, CancellationToken.None);

			var expectedContent = await File.ReadAllBytesAsync(Paths.GetTestDataFilePath("Output Song With Optional Data Missing.mp3"));

			AssertData(expectedSong, receivedSong);
			AssertStorageContent("2001 - Platinum Hits (CD 2)/04 - Hail Caesar.mp3", expectedContent);
		}

		[TestMethod]
		public async Task CreateSongMutation_DeletedSong_CreatesSongSuccessfully()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				ArtistId = 2,
				GenreId = 3,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				TrackNumber = 4,
				Duration = new TimeSpan(0, 5, 13),
				Rating = Rating.R4,
				BitRate = 320000,
				DeleteDate = new DateTimeOffset(2019, 12, 10, 07, 20, 25, TimeSpan.FromHours(2)),
				DeleteComment = "For a test",
			};

			var client = CreateClient<ISongsMutation>();

			// Act

			var newSongId = await client.CreateDeletedSong(newSongData, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newSongId);

			// Checking created song data

			var expectedSong = new OutputSongData
			{
				Id = 5,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				TrackNumber = 4,
				Duration = new TimeSpan(0, 5, 13),
				Disc = new OutputDiscData { Id = 1, },
				Artist = new OutputArtistData { Id = 2 },
				Genre = new OutputGenreData { Id = 3, },
				Rating = Rating.R4,
				BitRate = 320000,
				LastPlaybackTime = null,
				PlaybacksCount = 0,
				Playbacks = Array.Empty<OutputPlaybackData>(),
				DeleteDate = new DateTimeOffset(2019, 12, 10, 07, 20, 25, TimeSpan.FromHours(2)),
				DeleteComment = "For a test",
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSong = await songsQuery.GetSong(5, RequestedFields, CancellationToken.None);

			AssertData(expectedSong, receivedSong);
		}

		[TestMethod]
		public async Task CreateSongMutation_ForUnknownDisc_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 12345,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var client = CreateClient<ISongsMutation>();

			// Act

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));
			var createSongTask = client.CreateSong(newSongData, songContent, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("The disc with id of '12345' does not exist", exception.Message);

			// Checking that no songs were created.

			var expectedSongs = new[]
			{
				new OutputSongData { Id = 1, },
				new OutputSongData { Id = 2, },
				new OutputSongData { Id = 3, },
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			AssertData(expectedSongs, receivedSongs);
		}

		[TestMethod]
		public async Task CreateSongMutation_ForUnknownArtist_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				ArtistId = 12345,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var client = CreateClient<ISongsMutation>();

			// Act

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));
			var createSongTask = client.CreateSong(newSongData, songContent, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("The artist with id of '12345' does not exist", exception.Message);

			// Checking that no songs were created.

			var expectedSongs = new[]
			{
				new OutputSongData { Id = 1, },
				new OutputSongData { Id = 2, },
				new OutputSongData { Id = 3, },
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			AssertData(expectedSongs, receivedSongs);
		}

		[TestMethod]
		public async Task CreateSongMutation_ForUnknownGenre_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				GenreId = 12345,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var client = CreateClient<ISongsMutation>();

			// Act

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));
			var createSongTask = client.CreateSong(newSongData, songContent, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("The genre with id of '12345' does not exist", exception.Message);

			// Checking that no songs were created.

			var expectedSongs = new[]
			{
				new OutputSongData { Id = 1, },
				new OutputSongData { Id = 2, },
				new OutputSongData { Id = 3, },
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			AssertData(expectedSongs, receivedSongs);
		}

		[TestMethod]
		public async Task CreateSongMutation_ForBadContent_ReturnsError()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var client = CreateClient<ISongsMutation>();

			// Act

			await using var songContent = new MemoryStream(new byte[] { 0x01, 0x02, 0x03, });
			var createSongTask = client.CreateSong(newSongData, songContent, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createSongTask);
			Assert.AreEqual("Song content is invalid", exception.Message);

			// Checking that no songs were created.

			var expectedSongs = new[]
			{
				new OutputSongData { Id = 1, },
				new OutputSongData { Id = 2, },
				new OutputSongData { Id = 3, },
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			AssertData(expectedSongs, receivedSongs);
		}

		[TestMethod]
		public async Task CreateSongMutation_CreationOfSongInStorageFails_DoesNotCreateSongInRepository()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				GenreId = 1,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var storageServiceStub = new Mock<IStorageService>();
			storageServiceStub.Setup(x => x.StoreSong(It.IsAny<Song>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
				.Throws(new ServiceOperationFailedException("Exception from IStorageService.StoreSong()"));

			var client = CreateClient<ISongsMutation>(services => services.AddSingleton<IStorageService>(storageServiceStub.Object));

			// Act

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));
			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => client.CreateSong(newSongData, songContent, CancellationToken.None));
			Assert.AreEqual("Exception from IStorageService.StoreSong()", exception.Message);

			// Assert

			// Checking that no songs were created.

			var expectedSongs = new[]
			{
				new OutputSongData { Id = 1, },
				new OutputSongData { Id = 2, },
				new OutputSongData { Id = 3, },
			};

			var songsQuery = CreateClient<ISongsQuery>();
			var receivedSongs = await songsQuery.GetSongs(SongFields.Id, CancellationToken.None);

			AssertData(expectedSongs, receivedSongs);
		}

		[TestMethod]
		public async Task CreateSongMutation_CreationOfSongInRepositoryFails_DoesNotCreateSongInStorage()
		{
			// Arrange

			var newSongData = new InputSongData
			{
				DiscId = 1,
				ArtistId = null,
				GenreId = null,
				Title = "Hail Caesar",
				TreeTitle = "04 - Hail Caesar.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			var foldersRepositoryStub = new Mock<IFoldersRepository>();
			foldersRepositoryStub.Setup(x => x.GetFolder(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Folder { Id = 1, ParentFolderId = null, Name = "<ROOT>", });

			var discsRepositoryStub = new Mock<IDiscsRepository>();
			discsRepositoryStub.Setup(x => x.GetDisc(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Disc { Id = 1, TreeTitle = "2001 - Platinum Hits (CD 2)", FolderId = 1, });

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.Commit(It.IsAny<CancellationToken>()))
				.Throws(new ServiceOperationFailedException("Exception from IUnitOfWork.Commit()"));
			unitOfWorkStub.Setup(x => x.FoldersRepository).Returns(foldersRepositoryStub.Object);
			unitOfWorkStub.Setup(x => x.DiscsRepository).Returns(discsRepositoryStub.Object);
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(Mock.Of<ISongsRepository>());

			var client = CreateClient<ISongsMutation>(services => services.AddSingleton<IUnitOfWork>(unitOfWorkStub.Object));

			// Act

			await using var songContent = File.OpenRead(Paths.GetTestDataFilePath("Input Song With Filled ID3v2.3 tag.mp3"));
			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => client.CreateSong(newSongData, songContent, CancellationToken.None));
			Assert.AreEqual("Exception from IUnitOfWork.Commit()", exception.Message);

			// Assert

			// Sanity check, that we build paths correctly.
			Assert.IsTrue(File.Exists(GetFullContentPath("2001 - Platinum Hits (CD 2)/01 - Highway To Hell.mp3")));

			// Checking that no folders were created in the storage.
			Assert.IsFalse(File.Exists(GetFullContentPath("2001 - Platinum Hits (CD 2)/04 - Hail Caesar.mp3")));
		}
	}
}
