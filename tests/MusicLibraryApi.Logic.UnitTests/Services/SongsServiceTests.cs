using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Interfaces;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.Logic.UnitTests.Services
{
	[TestClass]
	public class SongsServiceTests
	{
		[TestMethod]
		public async Task CreateSong_RollbackOnErrorThrows_ThrowsOriginalException()
		{
			// Arrange

			var song = new Song
			{
				Title = "Some Song",
				TreeTitle = "Some Song",
				DiscId = 1,
			};

			var exception = new InvalidOperationException();

			var storageServiceStub = new Mock<IStorageService>();
			storageServiceStub.Setup(x => x.RollbackSongCreation(song, It.IsAny<CancellationToken>())).ThrowsAsync(new NotSupportedException());

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.Commit(It.IsAny<CancellationToken>())).ThrowsAsync(exception);
			unitOfWorkStub.Setup(x => x.SongsRepository).Returns(Mock.Of<ISongsRepository>());

			var target = new SongsService(unitOfWorkStub.Object, storageServiceStub.Object, Mock.Of<ILogger<SongsService>>());

			// Act

			await using var songContent = new MemoryStream();
			var caughtException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => target.CreateSong(song, songContent, CancellationToken.None));

			// Assert

			Assert.AreSame(exception, caughtException);
		}
	}
}
