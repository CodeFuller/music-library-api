using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.IntegrationTests.Comparers;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class FoldersTests : GraphQLTests
	{
		[TestMethod]
		public async Task FolderQuery_ForRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var subfolders = new[]
			{
				new OutputFolderData(2, "Foreign"),
				new OutputFolderData(1, "Russian"),
			};

			var discs = new[]
			{
				new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 1),
				new OutputDiscData(1, 2001, "Platinum Hits (CD 2)", "Platinum Hits", "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 2),
			};

			var expectedFolder = new OutputFolderData(0, "<ROOT>", subfolders, discs);

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(null, FolderFields.All, CancellationToken.None);

			// Assert

			var cmp = new FolderDataComparer().Compare(expectedFolder, receivedFolder);
			Assert.AreEqual(0, cmp, "Folders data does not match");
		}

		[TestMethod]
		public async Task FolderQuery_ForNonRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var subfolders = new[]
			{
				new OutputFolderData(6, "Empty folder"),
				new OutputFolderData(5, "Some subfolder"),
			};

			var discs = new[]
			{
				new OutputDiscData(5, 1997, "Proud Like A God", "Proud Like A God"),
				new OutputDiscData(3, 2000, "Don't Give Me Names", "Don't Give Me Names"),
				new OutputDiscData(4, null, "Rarities", String.Empty),
			};

			var expectedFolder = new OutputFolderData(4, "Guano Apes", subfolders, discs);

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(4, FolderFields.All, CancellationToken.None);

			// Assert

			var cmp = new FolderDataComparer().Compare(expectedFolder, receivedFolder);
			Assert.AreEqual(0, cmp, "Folders data does not match");
		}

		[TestMethod]
		public async Task FolderQuery_IfIncludeDeletedDiscs_ReturnsDeletedDiscs()
		{
			// Arrange

			var subfolders = new[]
			{
				new OutputFolderData(6, "Empty folder"),
				new OutputFolderData(5, "Some subfolder"),
			};

			var discs = new[]
			{
				new OutputDiscData(5, 1997, "Proud Like A God", "Proud Like A God"),
				new OutputDiscData(3, 2000, "Don't Give Me Names", "Don't Give Me Names"),
				new OutputDiscData(7, 2006, "Lost (T)apes", "Lost (T)apes", null, null, new DateTimeOffset(2019, 12, 03, 07, 57, 01, TimeSpan.FromHours(2)), "Deleted for a test"),
				new OutputDiscData(4, null, "Rarities", String.Empty),
			};

			var expectedFolder = new OutputFolderData(4, "Guano Apes", subfolders, discs);

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(4, FolderFields.All, CancellationToken.None, true);

			// Assert

			var cmp = new FolderDataComparer().Compare(expectedFolder, receivedFolder);
			Assert.AreEqual(0, cmp, "Folders data does not match");
		}

		[TestMethod]
		public async Task FolderQuery_ForUnknownFolder_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IFoldersQuery>();

			// Act

			var getSubfoldersTask = client.GetFolder(12345, FolderFields.All, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getSubfoldersTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreateFolder_ForRootParentFolder_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData("Belarussian", null);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(7, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(7, "Belarussian"),
				new OutputFolderData(2, "Foreign"),
				new OutputFolderData(1, "Russian"),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolderData = await foldersClient.GetFolder(null, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolderData.Subfolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_ForNonRootParentFolder_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData("Korn", 2);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(7, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(4, "Guano Apes"),
				new OutputFolderData(7, "Korn"),
				new OutputFolderData(3, "Rammstein"),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolderData = await foldersClient.GetFolder(2, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolderData.Subfolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_IfFolderAlreadyExists_ReturnsError()
		{
			// Arrange

			var folderData = new InputFolderData("Guano Apes", 2);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var createFolderTask = client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createFolderTask);
			Assert.AreEqual("Folder 'Guano Apes' already exists", exception.Message);

			// Checking that no changes to the folders were made

			var expectedFolders = new[]
			{
				new OutputFolderData(4, "Guano Apes"),
				new OutputFolderData(3, "Rammstein"),
			};

			var foldersClient = CreateClient<IFoldersQuery>();
			var receivedFolderData = await foldersClient.GetFolder(2, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			CollectionAssert.AreEqual(expectedFolders, receivedFolderData.Subfolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_IfFolderExistsUnderAnotherParent_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData("Guano Apes", 1);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(7, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(7, "Guano Apes"),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolderData = await foldersClient.GetFolder(1, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolderData.Subfolders.ToList(), new FolderDataComparer());
		}
	}
}
