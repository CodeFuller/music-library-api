using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Exceptions;
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

		[DataRow("Do You Like It?", "Do You Like It")]
		[DataRow("Another Me \"In Lack'ech\"", "Another Me 'In Lack'ech'")]
		[DataRow("Ezekiel 25:17", "Ezekiel 25-17")]
		[DataRow("Off/On (DrOff/On Remix)", "Off-On (DrOff-On Remix)")]
		[DataRow("New|sympho|type", "New-sympho-type")]
		[DataTestMethod]
		public async Task CreateFolder_PathContainsInvalidCharsFromPredefinedReplacements_ReplacesInvalidChars(string inputName, string expectedName)
		{
			// Arrange

			var fileSystemMock = new Mock<IFileSystemFacade>();
			fileSystemMock.Setup(x => x.DirectoryExists("c:\\music")).Returns(true);

			var target = new FileSystemContentStorage(fileSystemMock.Object, StubOptions);

			// Act

			await target.CreateFolder(new[] { "<ROOT>", inputName, }, CancellationToken.None);

			// Assert

			fileSystemMock.Verify(x => x.CreateDirectory($"c:\\music\\{expectedName}"), Times.Once());
		}

		[TestMethod]
		public async Task CreateFolder_PathContainsInvalidCharsNotFromPredefinedReplacements_RemovesInvalidChars()
		{
			// Arrange

			var fileSystemMock = new Mock<IFileSystemFacade>();
			fileSystemMock.Setup(x => x.DirectoryExists("c:\\music")).Returns(true);

			var target = new FileSystemContentStorage(fileSystemMock.Object, StubOptions);

			// Act

			await target.CreateFolder(new[] { "<ROOT>", @"Invalid Characters (<>*)", }, CancellationToken.None);

			// Assert

			fileSystemMock.Verify(x => x.CreateDirectory("c:\\music\\Invalid Characters ()"), Times.Once());
		}

		[TestMethod]
		public async Task CreateFolder_ResultFileNameIsEmpty_ThrowsServiceOperationFailedException()
		{
			// Arrange

			var fileSystemStub = new Mock<IFileSystemFacade>();
			fileSystemStub.Setup(x => x.DirectoryExists("c:\\music")).Returns(true);

			var target = new FileSystemContentStorage(fileSystemStub.Object, StubOptions);

			// Act

			Task CreateFolderCall() => target.CreateFolder(new[] { "<ROOT>", @"<>*", "Good Name" }, CancellationToken.None);

			// Assert

			await Assert.ThrowsExceptionAsync<ServiceOperationFailedException>(CreateFolderCall);
		}
	}
}
