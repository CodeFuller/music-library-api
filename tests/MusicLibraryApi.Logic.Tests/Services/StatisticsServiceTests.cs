using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.Logic.Tests.Services
{
	[TestClass]

	public class StatisticsServiceTests
	{
		[TestMethod]
		public async Task GetDiscArtistsNumber_ForDiscWithAllSongsFromOneArtist_CountsArtistAsDiscArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 1, TimeSpan.Zero, discId: 2, artistId: 2, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var discArtistsNumber = await target.GetDiscArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, discArtistsNumber);
		}

		[TestMethod]
		public async Task GetDiscArtistsNumber_SkipsDeletedDiscs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null,
					lastPlaybackTime: null, playbacksCount: 0, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var discArtistsNumber = await target.GetDiscArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(0, discArtistsNumber);
		}

		[TestMethod]
		public async Task GetDiscArtistsNumber_DoesNotCountArtistsOfDeletedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.Zero, discId: 1, artistId: 2, genreId: null, Rating.R1, bitRate: null,
					lastPlaybackTime: null, playbacksCount: 0, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var discArtistsNumber = await target.GetDiscArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(1, discArtistsNumber);
		}

		[TestMethod]
		public async Task GetDiscArtistsNumber_DoesNotCountMixedDiscArtists()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.Zero, discId: 1, artistId: 2, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var discArtistsNumber = await target.GetDiscArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(0, discArtistsNumber);
		}

		[TestMethod]
		public async Task GetDiscArtistsNumber_IfOneArtistHasMultipleDiscs_CountsArtistOnce()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.Zero, discId: 2, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var discArtistsNumber = await target.GetDiscArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(1, discArtistsNumber);
		}

		[TestMethod]
		public async Task GetDiscsNumber_SkipsDeletedDiscs()
		{
			// Arrange

			var discs = new[]
			{
				new Disc(year: 2019, title: "title1", treeTitle: "treeTitle1", albumTitle: "albumTitle1", folderId: 1, albumId: null, albumOrder: null, deleteDate: null),
				new Disc(year: 2018, title: "title2", treeTitle: "treeTitle2", albumTitle: "albumTitle2", folderId: 1, albumId: null, albumOrder: null, deleteDate: DateTimeOffset.FromUnixTimeSeconds(10)),
				new Disc(year: 2018, title: "title3", treeTitle: "treeTitle3", albumTitle: "albumTitle3", folderId: 1, albumId: null, albumOrder: null, deleteDate: null),
			};

			var discsRepositoryStub = new Mock<IDiscsRepository>();
			discsRepositoryStub.Setup(x => x.GetAllDiscs(It.IsAny<CancellationToken>())).ReturnsAsync(discs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.DiscsRepository).Returns(discsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var discsNumber = await target.GetDiscsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, discsNumber);
		}

		[TestMethod]
		public async Task GetSongsNumber_SkipsDeletedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.Zero, discId: 1, artistId: 2, genreId: null, Rating.R1, bitRate: null,
					lastPlaybackTime: null, playbacksCount: 0, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.Zero, discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var songsNumber = await target.GetSongsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, songsNumber);
		}

		[TestMethod]
		public async Task GetSongsDuration_SkipsDeletedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.FromSeconds(10), discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.FromSeconds(20), discId: 1, artistId: 2, genreId: null, Rating.R1, bitRate: null,
					lastPlaybackTime: null, playbacksCount: 0, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.FromSeconds(30), discId: 1, artistId: 1, genreId: null, Rating.R1, bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var songsDuration = await target.GetSongsDuration(CancellationToken.None);

			// Assert

			Assert.AreEqual(TimeSpan.FromSeconds(40), songsDuration);
		}

		[TestMethod]
		public async Task GetPlaybacksDuration_IncludesDeletedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.FromSeconds(10), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 1),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.FromSeconds(20), discId: 1, artistId: 2, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 2, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.FromSeconds(30), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 3),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var playbacksDuration = await target.GetPlaybacksDuration(CancellationToken.None);

			// Assert

			Assert.AreEqual(TimeSpan.FromSeconds(140), playbacksDuration);
		}

		[TestMethod]
		public async Task GetPlaybacksNumber_IncludesDeletedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.FromSeconds(10), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 1),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.FromSeconds(20), discId: 1, artistId: 2, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 2, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.FromSeconds(30), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 3),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var playbacksNumber = await target.GetPlaybacksNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(6, playbacksNumber);
		}

		[TestMethod]
		public async Task GetUnheardSongsNumber_SkipsDeletedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.FromSeconds(10), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.FromSeconds(20), discId: 1, artistId: 2, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 0, deleteDate: new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero)),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.FromSeconds(30), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var unheardSongsNumber = await target.GetUnheardSongsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, unheardSongsNumber);
		}

		[TestMethod]
		public async Task GetUnheardSongsNumber_SkipsListenedSongs()
		{
			// Arrange

			var songs = new[]
			{
				new Song("title1", "treeTitle1", trackNumber: 1, TimeSpan.FromSeconds(10), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
				new Song("title2", "treeTitle2", trackNumber: 2, TimeSpan.FromSeconds(20), discId: 1, artistId: 2, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 5),
				new Song("title3", "treeTitle3", trackNumber: 1, TimeSpan.FromSeconds(30), discId: 1, artistId: 1, genreId: null, Rating.R1,
					bitRate: null, lastPlaybackTime: null, playbacksCount: 0),
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var unheardSongsNumber = await target.GetUnheardSongsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, unheardSongsNumber);
		}
	}
}
