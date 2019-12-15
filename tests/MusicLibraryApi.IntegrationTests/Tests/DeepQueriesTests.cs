using System;
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
				new OutputFolderData(id: 4, name: "Rammstein"),
			};

			var expectedSubfolders22 = new[]
			{
				new OutputFolderData(id: 7, name: "Empty folder"),
				new OutputFolderData(id: 6, name: "Some subfolder"),
			};

			var expectedDiscs2 = new[]
			{
				new OutputDiscData(id: 5, year: 1997, title: "Proud Like A God", treeTitle: "1997 - Proud Like A God", albumTitle: "Proud Like A God"),
				new OutputDiscData(id: 3, year: 2000, title: "Don't Give Me Names", treeTitle: "2000 - Don't Give Me Names", albumTitle: "Don't Give Me Names"),
				new OutputDiscData(id: 4, title: "Rarities", treeTitle: "Rarities", albumTitle: String.Empty),
			};

			var expectedSubfolders1 = new[]
			{
				new OutputFolderData(id: 3, name: "Foreign", subfolders: expectedSubfolders21, discs: Array.Empty<OutputDiscData>()),
				new OutputFolderData(id: 5, name: "Guano Apes", subfolders: expectedSubfolders22, discs: expectedDiscs2),
				new OutputFolderData(id: 2, name: "Russian", subfolders: Array.Empty<OutputFolderData>(), discs: Array.Empty<OutputDiscData>()),
			};

			var expectedSongs1 = new[]
			{
				new OutputSongData(id: 2, title: "Highway To Hell", treeTitle: "01 - Highway To Hell.mp3", trackNumber: 1, duration: new TimeSpan(0, 3, 28),
					disc: new OutputDiscData(id: 1), artist: new OutputArtistData(id: 2, name: "AC/DC"), genre: new OutputGenreData(id: 1, name: "Russian Rock"), rating: Rating.R6, bitRate: 320000,
					lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), playbacksCount: 1,
					playbacks: new[] { new OutputPlaybackData(id: 2) }),

				new OutputSongData(id: 1, title: "Hell's Bells", treeTitle: "02 - Hell's Bells.mp3", trackNumber: 2, duration: new TimeSpan(0, 5, 12),
					disc: new OutputDiscData(id: 1), artist: null, genre: new OutputGenreData(id: 2, name: "Nu Metal"), rating: Rating.R4, bitRate: 320000,
					lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), playbacksCount: 2,
					playbacks: new[] { new OutputPlaybackData(id: 3), new OutputPlaybackData(id: 1), }),
			};

			var expectedSongs2 = new[]
			{
				new OutputSongData(id: 3, title: "Are You Ready?", treeTitle: "03 - Are You Ready?.mp3", duration: new TimeSpan(0, 4, 09),
					disc: new OutputDiscData(id: 2), artist: new OutputArtistData(id: 1, name: "Nautilus Pompilius"), genre: null, playbacksCount: 0,
					playbacks: Array.Empty<OutputPlaybackData>()),
			};

			var expectedDiscs1 = new[]
			{
				new OutputDiscData(id: 2, year: 2001, title: "Platinum Hits (CD 1)", treeTitle: "2001 - Platinum Hits (CD 1)",
					albumTitle: "Platinum Hits", albumId: "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", albumOrder: 1, folder: new OutputFolderData(id: 1, name: "<ROOT>"),
					songs: expectedSongs2),

				new OutputDiscData(id: 1, year: 2001, title: "Platinum Hits (CD 2)", treeTitle: "2001 - Platinum Hits (CD 2)",
					albumTitle: "Platinum Hits", albumId: "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", albumOrder: 2, folder: new OutputFolderData(id: 1, name: "<ROOT>"),
					songs: expectedSongs1),
			};

			var expectedFolder = new OutputFolderData(id: 1, name: "<ROOT>", subfolders: expectedSubfolders1, discs: expectedDiscs1);

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

			var expectedDisc = new OutputDiscData(id: 1, year: 2001, title: "Platinum Hits (CD 2)", treeTitle: "2001 - Platinum Hits (CD 2)",
					albumTitle: "Platinum Hits", albumId: "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", albumOrder: 2, folder: new OutputFolderData(id: 1, name: "<ROOT>"));

			var expectedSong = new OutputSongData(id: 2, title: "Highway To Hell", treeTitle: "01 - Highway To Hell.mp3", trackNumber: 1, duration: new TimeSpan(0, 3, 28),
				disc: expectedDisc, artist: new OutputArtistData(2, "AC/DC"), genre: new OutputGenreData(id: 1, name: "Russian Rock"), rating: Rating.R6, bitRate: 320000,
				lastPlaybackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), playbacksCount: 1);

			var expectedPlayback = new OutputPlaybackData(id: 2, playbackTime: new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), song: expectedSong);

			var client = CreateClient<IPlaybacksQuery>();

			// Act

			var receivedPlayback = await client.GetPlayback(2, requestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedPlayback, receivedPlayback);
		}
	}
}
