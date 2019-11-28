using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests
{
	public sealed partial class GraphQLTests
	{
		private class FolderDataComparer : IComparer
		{
			public int Compare(object? x, object? y)
			{
				// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
				var f1 = (OutputFolderData?)x;
				var f2 = (OutputFolderData?)y;

				if (Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
				{
					return 0;
				}

				if (Object.ReferenceEquals(f1, null))
				{
					return -1;
				}

				if (Object.ReferenceEquals(f2, null))
				{
					return 1;
				}

				var cmp = Nullable.Compare(f1.Id, f2.Id);
				if (cmp != 0)
				{
					return cmp;
				}

				cmp = String.Compare(f1.Name, f2.Name, StringComparison.Ordinal);
				if (cmp != 0)
				{
					return cmp;
				}

				return Nullable.Compare(f1.ParentFolderId, f2.ParentFolderId);
			}
		}

		[TestMethod]
		public async Task SubfoldersQuery_ForRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var expectedFolders = new[]
			{
				new OutputFolderData(5, "Foreign", null),
				new OutputFolderData(1, "Russian", null),
				new OutputFolderData(3, "Сборники", null),
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
				new OutputFolderData(7, "AC-DC", 5),
				new OutputFolderData(6, "Korn", 5),
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await client.GetSubfolders(5, FolderFields.All, CancellationToken.None);

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
		public async Task CreateFolder_ForRootParentFolder_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData("Belarussian", null);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(8, "Belarussian", null),
				new OutputFolderData(5, "Foreign", null),
				new OutputFolderData(1, "Russian", null),
				new OutputFolderData(3, "Сборники", null),
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

			var folderData = new InputFolderData("Guano Apes", 5);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(7, "AC-DC", 5),
				new OutputFolderData(8, "Guano Apes", 5),
				new OutputFolderData(6, "Korn", 5),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await foldersClient.GetSubfolders(5, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}

		[TestMethod]
		public async Task CreateFolder_IfFolderAlreadyExists_ReturnsError()
		{
			// Arrange

			var folderData = new InputFolderData("Korn", 5);

			var client = CreateClient<IFoldersMutation>();

			// Act

			var createFolderTask = client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createFolderTask);
			Assert.AreEqual("Folder 'Korn' already exists", exception.Message);

			// Checking that no changes to the folders were made

			var expectedFolders = new[]
			{
				new OutputFolderData(7, "AC-DC", 5),
				new OutputFolderData(6, "Korn", 5),
			};

			var foldersClient = CreateClient<IFoldersQuery>();
			var receivedFolders = await foldersClient.GetSubfolders(5, FolderFields.All, CancellationToken.None);

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

			Assert.AreEqual(8, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData(4, "Best", 3),
				new OutputFolderData(8, "Nautilus Pompilius", 3),
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolders = await foldersClient.GetSubfolders(3, FolderFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedFolders, receivedFolders.ToList(), new FolderDataComparer());
		}
	}
}
