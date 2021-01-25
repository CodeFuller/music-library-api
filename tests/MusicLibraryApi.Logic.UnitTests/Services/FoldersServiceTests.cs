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

namespace MusicLibraryApi.Logic.UnitTests.Services
{
	[TestClass]
	public class FoldersServiceTests
	{
		[TestMethod]
		public async Task CreateFolder_RollbackOnErrorThrows_ThrowsOriginalException()
		{
			// Arrange

			var folder = new Folder
			{
				Name = "Some Folder",
				ParentFolderId = 1,
			};

			var exception = new InvalidOperationException();

			var storageServiceStub = new Mock<IStorageService>();
			storageServiceStub.Setup(x => x.RollbackFolderCreation(folder, It.IsAny<CancellationToken>())).ThrowsAsync(new NotSupportedException());

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.Commit(It.IsAny<CancellationToken>())).ThrowsAsync(exception);
			unitOfWorkStub.Setup(x => x.FoldersRepository).Returns(Mock.Of<IFoldersRepository>());

			var target = new FoldersService(unitOfWorkStub.Object, storageServiceStub.Object, Mock.Of<ILogger<FoldersService>>());

			// Act

			var caughtException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => target.CreateFolder(folder, CancellationToken.None));

			// Assert

			Assert.AreSame(exception, caughtException);
		}
	}
}
