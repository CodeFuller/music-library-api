using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Logic.Internal;
using MusicLibraryApi.Logic.Settings;

namespace MusicLibraryApi.Logic.Tests.Internal
{
	[TestClass]
	public class FileSystemContentStorageTests
	{
		private static IOptions<FileSystemStorageSettings> StubOptions => Options.Create(new FileSystemStorageSettings
		{
			Root = "c:\\music",
		});

		[TestMethod]
		public async Task CreateFolder_PathContainsInvalidChars_RemovesInvalidChars()
		{
			// Arrange

			var fileSystemMock = new Mock<IFileSystemFacade>();
			fileSystemMock.Setup(x => x.DirectoryExists("c:\\music")).Returns(true);

			var target = new FileSystemContentStorage(fileSystemMock.Object, StubOptions);

			// Act

			await target.CreateFolder(new[] { "<ROOT>", @"Invalid Characters (""<>|:*?\/)", }, CancellationToken.None);

			// Assert

			fileSystemMock.Verify(x => x.CreateDirectory("c:\\music\\Invalid Characters ()"), Times.Once());
		}

		[TestMethod]
		public async Task CreateFolder_WholePathPartConsistsOfInvalidCharacters_ReplacesPathPartWithUnderscore()
		{
			// Arrange

			var fileSystemMock = new Mock<IFileSystemFacade>();
			fileSystemMock.Setup(x => x.DirectoryExists("c:\\music")).Returns(true);

			var target = new FileSystemContentStorage(fileSystemMock.Object, StubOptions);

			// Act

			await target.CreateFolder(new[] { "<ROOT>", @"""<>|:*?\/", "Good Name" }, CancellationToken.None);

			// Assert

			fileSystemMock.Verify(x => x.CreateDirectory("c:\\music\\_\\Good Name"), Times.Once());
		}
	}
}
