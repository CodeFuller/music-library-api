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
		public async Task SubfoldersQuery_ForRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var expectedFolders = new[]
			{
				new OutputFolderData(3, "Foreign", null),
				new OutputFolderData(1, "Russian", null),
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await client.GetSubfolders(null, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task SubfoldersQuery_ForNonRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var expectedFolders = new[]
			{
				new OutputFolderData(5, "AC-DC", 3),
				new OutputFolderData(4, "Korn", 3),
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await client.GetSubfolders(3, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task SubfoldersQuery_ForUnknownFolder_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IFoldersQuery>();

			// Act

			var getSubfoldersTask = client.GetSubfolders(12345, FolderFields.All, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getSubfoldersTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task DiscsQuery_ForRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var expectedDiscs = new[]
			{
				new OutputDiscData(4, null, "Foreign Best", null, null, null, null),
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await client.GetFolderDiscs(null, DiscFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedDiscs, receivedFolders.ToList(), new DiscDataComparer());
		}

		[TestMethod]
		public async Task DiscsQuery_ForNonRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var expectedFolders = new[]
			{
				new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1, new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(3, 2001, "Platinum Hits (CD 2)", "Platinum Hits", 2, new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)), "Boring"),
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await client.GetFolderDiscs(5, DiscFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new DiscDataComparer());
		}

		[TestMethod]
		public async Task DiscsQuery_ForUnknownFolder_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IFoldersQuery>();

			// Act

			var getSubfoldersTask = client.GetFolderDiscs(12345, DiscFields.All, CancellationToken.None);

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

			Assert.AreEqual(6, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(6, "Belarussian", null),
				new OutputFolderData(3, "Foreign", null),
				new OutputFolderData(1, "Russian", null),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await foldersClient.GetSubfolders(null, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_ForNonRootParentFolder_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData("Guano Apes", 3);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(6, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(5, "AC-DC", 3),
				new OutputFolderData(6, "Guano Apes", 3),
				new OutputFolderData(4, "Korn", 3),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await foldersClient.GetSubfolders(3, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_IfFolderAlreadyExists_ReturnsError()
		{
			// Arrange

			var folderData = new InputFolderData("Korn", 3);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var createFolderTask = client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createFolderTask);
			Assert.AreEqual("Folder 'Korn' already exists", exception.Message);

			// Checking that no changes to the folders were made

			var expectedFolders = new[]
			{
				new OutputFolderData(5, "AC-DC", 3),
				new OutputFolderData(4, "Korn", 3),
			};

			var foldersClient = CreateClient<IFoldersQuery>();
			var receivedFolders = await foldersClient.GetSubfolders(3, FolderFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_IfFolderExistsUnderAnotherParent_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData("Nautilus Pompilius", 3);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(6, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(5, "AC-DC", 3),
				new OutputFolderData(4, "Korn", 3),
				new OutputFolderData(6, "Nautilus Pompilius", 3),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await foldersClient.GetSubfolders(3, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}
	}
}
