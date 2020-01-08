using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class DeepQueriesTests : GraphQLTests
	{
		[TestMethod]
		public async Task DeepDownwardQuery_ReturnsDataCorrectly()
		{
			// Arrange

			var subfolderFields = FolderFields.All + FolderFields.Subfolders(FolderFields.All) + FolderFields.Discs(DiscFields.All);
			var songFields = SongFields.All + SongFields.Artist(ArtistFields.All) + SongFields.Genre(GenreFields.All) + SongFields.Disc(DiscFields.Id) + SongFields.Playbacks(PlaybackFields.Id);
			var discFields = DiscFields.All + DiscFields.Songs(songFields) + DiscFields.Folder(FolderFields.All);
			var requestedFields = FolderFields.All + FolderFields.Subfolders(subfolderFields) + FolderFields.Discs(discFields);

			var expectedSubfolders21 = new[]
			{
				new OutputFolderData { Id = 4, Name = "Rammstein", },
			};

			var expectedSubfolders22 = new[]
			{
				new OutputFolderData { Id = 7, Name = "Empty folder", },
				new OutputFolderData { Id = 6, Name = "Some subfolder", },
			};

			var expectedDiscs2 = new[]
			{
				new OutputDiscData
				{
					Id = 5,
					Year = 1997,
					Title = "Proud Like A God",
					TreeTitle = "1997 - Proud Like A God",
					AlbumTitle = "Proud Like A God",
				},

				new OutputDiscData
				{
					Id = 3,
					Year = 2000,
					Title = "Don't Give Me Names",
					TreeTitle = "2000 - Don't Give Me Names",
					AlbumTitle = "Don't Give Me Names",
				},

				new OutputDiscData
				{
					Id = 4,
					Title = "Rarities",
					TreeTitle = "Rarities",
					AlbumTitle = String.Empty,
				},
			};

			var expectedSubfolders1 = new[]
			{
				new OutputFolderData
				{
					Id = 3,
					Name = "Foreign",
					Subfolders = expectedSubfolders21,
					Discs = Array.Empty<OutputDiscData>(),
				},

				new OutputFolderData
				{
					Id = 5,
					Name = "Guano Apes",
					Subfolders = expectedSubfolders22,
					Discs = expectedDiscs2,
				},

				new OutputFolderData
				{
					Id = 2,
					Name = "Russian",
					Subfolders = Array.Empty<OutputFolderData>(),
					Discs = Array.Empty<OutputDiscData>(),
				},
			};

			var expectedSongs1 = new[]
			{
				new OutputSongData
				{
					Id = 2,
					Title = "Highway To Hell",
					TreeTitle = "01 - Highway To Hell.mp3",
					TrackNumber = 1,
					Duration = new TimeSpan(0, 3, 28),
					Disc = new OutputDiscData { Id = 1, },
					Artist = new OutputArtistData { Id = 2, Name = "AC/DC", },
					Genre = new OutputGenreData { Id = 1, Name = "Russian Rock", },
					Rating = Rating.R6,
					BitRate = 320000,
					Size = 946,
					LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
					PlaybacksCount = 1,
					Playbacks = new[] { new OutputPlaybackData { Id = 2, } },
				},

				new OutputSongData
				{
					Id = 1,
					Title = "Hell's Bells",
					TreeTitle = "02 - Hell's Bells.mp3",
					TrackNumber = 2,
					Duration = new TimeSpan(0, 5, 12),
					Disc = new OutputDiscData { Id = 1, },
					Genre = new OutputGenreData { Id = 2, Name = "Nu Metal", },
					Rating = Rating.R4,
					BitRate = 320000,
					Size = 1243,
					LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)),
					PlaybacksCount = 2,
					Playbacks = new[] { new OutputPlaybackData { Id = 3, }, new OutputPlaybackData { Id = 1, }, },
				},
			};

			var expectedSongs2 = new[]
			{
				new OutputSongData
				{
					Id = 3,
					Title = "Are You Ready?",
					TreeTitle = "03 - Are You Ready?.mp3",
					Duration = new TimeSpan(0, 4, 09),
					Disc = new OutputDiscData { Id = 2, },
					Artist = new OutputArtistData { Id = 1, Name = "Nautilus Pompilius", },
					Size = 673,
					PlaybacksCount = 0,
					Playbacks = Array.Empty<OutputPlaybackData>(),
				},
			};

			var expectedDiscs1 = new[]
			{
				new OutputDiscData
				{
					Id = 2,
					Year = 2001,
					Title = "Platinum Hits (CD 1)",
					TreeTitle = "2001 - Platinum Hits (CD 1)",
					AlbumTitle = "Platinum Hits",
					AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
					AlbumOrder = 1,
					Folder = new OutputFolderData { Id = 1, Name = "<ROOT>", },
					Songs = expectedSongs2,
				},

				new OutputDiscData
				{
					Id = 1,
					Year = 2001,
					Title = "Platinum Hits (CD 2)",
					TreeTitle = "2001 - Platinum Hits (CD 2)",
					AlbumTitle = "Platinum Hits",
					AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
					AlbumOrder = 2,
					Folder = new OutputFolderData { Id = 1, Name = "<ROOT>", },
					Songs = expectedSongs1,
				},
			};

			var expectedFolder = new OutputFolderData
			{
				Id = 1,
				Name = "<ROOT>",
				Subfolders = expectedSubfolders1,
				Discs = expectedDiscs1,
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(null, requestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedFolder, receivedFolder);
		}

		[TestMethod]
		public async Task DeepUpwardQuery_ReturnsDataCorrectly()
		{
			var songFields = SongFields.All + SongFields.Disc(DiscFields.All + DiscFields.Folder(FolderFields.All)) + SongFields.Artist(ArtistFields.All) + SongFields.Genre(GenreFields.All);
			var requestedFields = PlaybackFields.All + PlaybackFields.Song(songFields);

			var expectedDisc = new OutputDiscData
			{
				Id = 1,
				Year = 2001,
				Title = "Platinum Hits (CD 2)",
				TreeTitle = "2001 - Platinum Hits (CD 2)",
				AlbumTitle = "Platinum Hits",
				AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
				AlbumOrder = 2,
				Folder = new OutputFolderData { Id = 1, Name = "<ROOT>", },
			};

			var expectedSong = new OutputSongData
			{
				Id = 2,
				Title = "Highway To Hell",
				TreeTitle = "01 - Highway To Hell.mp3",
				TrackNumber = 1,
				Duration = new TimeSpan(0, 3, 28),
				Disc = expectedDisc,
				Artist = new OutputArtistData { Id = 2, Name = "AC/DC", },
				Genre = new OutputGenreData { Id = 1, Name = "Russian Rock", },
				Rating = Rating.R6,
				BitRate = 320000,
				Size = 946,
				LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
				PlaybacksCount = 1,
			};

			var expectedPlayback = new OutputPlaybackData
			{
				Id = 2,
				PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
				Song = expectedSong,
			};

			var client = CreateClient<IPlaybacksQuery>();

			// Act

			var receivedPlayback = await client.GetPlayback(2, requestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedPlayback, receivedPlayback);
		}

		[TestMethod]
		public async Task CreateDeepSongContent_CreatesContentCorrectly()
		{
			// Arrange

			var foldersClient = CreateClient<IFoldersMutation>();
			var discsClient = CreateClient<IDiscsMutation>();
			var songsClient = CreateClient<ISongsMutation>();

			// Act

			// Creating folder.
			var folderData = new InputFolderData { Name = "Some Folder", ParentFolderId = 6, };
			var folderId = await foldersClient.CreateFolder(folderData, CancellationToken.None);
			Assert.AreEqual(8, folderId);

			// Creating disc.
			var discData = new InputDiscData
			{
				FolderId = 8,
				Title = "Some Disc Title",
				TreeTitle = "Some Disc TreeTitle",
				AlbumTitle = "Some Disc AlbumTitle",
			};
			var discId = await discsClient.CreateDisc(discData, CancellationToken.None);
			Assert.AreEqual(8, discId);

			// Creating song.
			var songData = new InputSongData
			{
				DiscId = 8,
				Title = "Some Song Title",
				TreeTitle = "Some Song TreeTitle.mp3",
				Duration = new TimeSpan(0, 5, 13),
			};

			await using var contentStream = new MemoryStream(new byte[] { 0x01, 0x02, 0x03, });
			await songsClient.CreateSong(songData, contentStream, CancellationToken.None);

			// Assert

			AssertSongContent("Guano Apes/Some subfolder/Some Folder/Some Disc TreeTitle/Some Song TreeTitle.mp3", new byte[] { 0x01, 0x02, 0x03, });
		}
	}
}
