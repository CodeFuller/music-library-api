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
		public async Task GetArtistsNumber_ForDeletedSongs_DoesNotCountArtists()
		{
			// Arrange

			var songs = new[]
			{
				new Song { ArtistId = 1, },
				new Song { ArtistId = 2, DeleteDate = DateTimeOffset.FromUnixTimeSeconds(10), },
				new Song { ArtistId = 3, },
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var artistsNumber = await target.GetArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, artistsNumber);
		}

		[TestMethod]
		public async Task GetArtistsNumber_ForSongsWithoutArtist_DoesNotCountMissingArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song { ArtistId = 1, },
				new Song { ArtistId = null, },
				new Song { ArtistId = 2, },
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var artistsNumber = await target.GetArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, artistsNumber);
		}

		[TestMethod]
		public async Task GetArtistsNumber_ForSongsWithCountedArtist_DoesNotCountArtists()
		{
			// Arrange

			var songs = new[]
			{
				new Song { ArtistId = 1, },
				new Song { ArtistId = 2, },
				new Song { ArtistId = 1, },
			};

			var songsRepositoryStub = new Mock<ISongsRepository>();
			songsRepositoryStub.Setup(x => x.GetAllSongs(It.IsAny<CancellationToken>())).ReturnsAsync(songs);

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(songsRepositoryStub.Object);

			var target = new StatisticsService(unitOfWorkStub.Object);

			// Act

			var artistsNumber = await target.GetArtistsNumber(CancellationToken.None);

			// Assert

			Assert.AreEqual(2, artistsNumber);
		}

		[TestMethod]
		public async Task GetDiscArtistsNumber_ForDiscWithAllSongsFromOneArtist_CountsArtistAsDiscArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song { DiscId = 1, ArtistId = 1, },
				new Song { DiscId = 2, ArtistId = 2, },
				new Song { DiscId = 1, ArtistId = 1, },
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
		public async Task GetDiscArtistsNumber_ForDeletedDiscs_DoesNotCountArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song { DiscId = 1, ArtistId = 1, DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
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
		public async Task GetDiscArtistsNumber_ForDeletedDiscSongs_DoesNotCountArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song { DiscId = 1, ArtistId = 1, },
				new Song { DiscId = 1, ArtistId = 2, DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
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
		public async Task GetDiscArtistsNumber_ForDiscsWithMixedArtists_DoesNotCountArtists()
		{
			// Arrange

			var songs = new[]
			{
				new Song { DiscId = 1, ArtistId = 1, },
				new Song { DiscId = 1, ArtistId = 2, },
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
		public async Task GetDiscArtistsNumber_IfSomeDiscSongsDoNotHaveArtist_DoesNotCountDiscArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song { DiscId = 1, ArtistId = 1, },
				new Song { DiscId = 1, ArtistId = null, },
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
		public async Task GetDiscArtistsNumber_ForDiscWithoutArtist_DoesNotCountDiscArtist()
		{
			// Arrange

			var songs = new[]
			{
				new Song { DiscId = 1, ArtistId = null, },
				new Song { DiscId = 1, ArtistId = null, },
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
				new Song { DiscId = 1, ArtistId = 1, },
				new Song { DiscId = 2, ArtistId = 1, },
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
		public async Task GetDiscsNumber_ForDeletedDiscs_DoesNotCount()
		{
			// Arrange

			var discs = new[]
			{
				new Disc(),
				new Disc { DeleteDate = DateTimeOffset.FromUnixTimeSeconds(10), },
				new Disc(),
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
		public async Task GetSongsNumber_ForDeletedSongs_DoesNotCount()
		{
			// Arrange

			var songs = new[]
			{
				new Song(),
				new Song { DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
				new Song(),
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
		public async Task GetSongsDuration_ForDeletedSongs_DoesNotCount()
		{
			// Arrange

			var songs = new[]
			{
				new Song { Duration = TimeSpan.FromSeconds(10), },
				new Song { Duration = TimeSpan.FromSeconds(20), DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
				new Song { Duration = TimeSpan.FromSeconds(30), },
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
		public async Task GetPlaybacksDuration_ForDeletedSongs_AddsToPlaybacksDuration()
		{
			// Arrange

			var songs = new[]
			{
				new Song { Duration = TimeSpan.FromSeconds(10), PlaybacksCount = 1, },
				new Song { Duration = TimeSpan.FromSeconds(20), PlaybacksCount = 2, DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
				new Song { Duration = TimeSpan.FromSeconds(30), PlaybacksCount = 3, },
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
		public async Task GetPlaybacksNumber_ForDeletedSongs_AddsToPlaybacksNumber()
		{
			// Arrange

			var songs = new[]
			{
				new Song { PlaybacksCount = 1, },
				new Song { PlaybacksCount = 2, DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
				new Song { PlaybacksCount = 3, },
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
		public async Task GetUnheardSongsNumber_ForDeletedSongs_DoesNotCount()
		{
			// Arrange

			var songs = new[]
			{
				new Song { PlaybacksCount = 0, },
				new Song { PlaybacksCount = 0, DeleteDate = new DateTimeOffset(2019, 12, 14, 18, 00, 00, TimeSpan.Zero), },
				new Song { PlaybacksCount = 0, },
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
		public async Task GetUnheardSongsNumber_ForListenedSongs_DoesNotCount()
		{
			// Arrange

			var songs = new[]
			{
				new Song { PlaybacksCount = 0, },
				new Song { PlaybacksCount = 5, },
				new Song { PlaybacksCount = 0, },
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
