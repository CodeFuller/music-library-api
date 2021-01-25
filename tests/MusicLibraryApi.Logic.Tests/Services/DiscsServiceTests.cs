using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Interfaces;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.Logic.Tests.Services
{
	[TestClass]
	public class DiscsServiceTests
	{
		[TestMethod]
		public async Task CreateDisc_RollbackOnErrorThrows_ThrowsOriginalException()
		{
			// Arrange

			var disc = new Disc
			{
				Title = "Some Disc",
				TreeTitle = "Some Disc",
				AlbumTitle = "Some Disc",
				FolderId = 1,
			};

			var exception = new InvalidOperationException();

			var storageServiceStub = new Mock<IStorageService>();
			storageServiceStub.Setup(x => x.RollbackDiscCreation(disc, It.IsAny<CancellationToken>())).ThrowsAsync(new NotSupportedException());

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.Commit(It.IsAny<CancellationToken>())).ThrowsAsync(exception);
			unitOfWorkStub.Setup(x => x.DiscsRepository).Returns(Mock.Of<IDiscsRepository>());

			var target = new DiscsService(unitOfWorkStub.Object, storageServiceStub.Object, Mock.Of<ILogger<DiscsService>>());

			// Act

			var caughtException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => target.CreateDisc(disc, CancellationToken.None));

			// Assert

			Assert.AreSame(exception, caughtException);
		}
	}
}
