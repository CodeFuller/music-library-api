using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class FoldersTests : GraphQLTests
	{
		private static QueryFieldSet<OutputFolderData> RequestedFields => FolderFields.All + FolderFields.Subfolders(FolderFields.All) + FolderFields.Discs(DiscFields.All);

		[TestMethod]
		public async Task FolderQuery_ForRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var subfolders = new[]
			{
				new OutputFolderData { Id = 3, Name = "Foreign", },
				new OutputFolderData { Id = 5, Name = "Guano Apes", },
				new OutputFolderData { Id = 2, Name = "Russian", },
			};

			var discs = new[]
			{
				new OutputDiscData
				{
					Id = 2,
					Year = 2001,
					Title = "Platinum Hits (CD 1)",
					TreeTitle = "2001 - Platinum Hits (CD 1)",
					AlbumTitle = "Platinum Hits",
					AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
					AlbumOrder = 1,
				},

				new OutputDiscData
				{
					Id = 1,
					Year = 2001,
					Title = "Platinum Hits (CD 2)",
					TreeTitle = "2001 - Platinum Hits (CD 2)",
					AlbumTitle = "Platinum Hits",
					AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
					AlbumOrder = 2,
				},
			};

			var expectedFolder = new OutputFolderData
			{
				Id = 1,
				Name = "<ROOT>",
				Subfolders = subfolders,
				Discs = discs,
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(null, RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedFolder, receivedFolder);
		}

		[TestMethod]
		public async Task FolderQuery_ForNonRootFolder_ReturnsCorrectData()
		{
			// Arrange

			var subfolders = new[]
			{
				new OutputFolderData { Id = 7, Name = "Empty folder", },
				new OutputFolderData { Id = 6, Name = "Some subfolder", },
			};

			var discs = new[]
			{
				new OutputDiscData
				{
					Id = 5,
					Year = 1997,
					Title = "Proud Like A God",
					TreeTitle = "1997 - Proud Like A God",
					AlbumTitle = "Proud Like A God",
				},

				new OutputDiscData
				{
					Id = 3,
					Year = 2000,
					Title = "Don't Give Me Names",
					TreeTitle = "2000 - Don't Give Me Names",
					AlbumTitle = "Don't Give Me Names",
				},

				new OutputDiscData
				{
					Id = 4,
					Title = "Rarities",
					TreeTitle = "Rarities",
					AlbumTitle = String.Empty,
				},
			};

			var expectedFolder = new OutputFolderData
			{
				Id = 5,
				Name = "Guano Apes",
				Subfolders = subfolders,
				Discs = discs,
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(5, RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedFolder, receivedFolder);
		}

		[TestMethod]
		public async Task FolderQuery_IfIncludeDeletedDiscs_ReturnsDeletedDiscs()
		{
			// Arrange

			var discs = new[]
			{
				new OutputDiscData { Id = 5, },
				new OutputDiscData { Id = 3, },
				new OutputDiscData { Id = 7, },
				new OutputDiscData { Id = 4, },
			};

			var expectedFolder = new OutputFolderData
			{
				Id = 5,
				Name = "Guano Apes",
				Discs = discs,
			};

			var client = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await client.GetFolder(5, FolderFields.All + FolderFields.Discs(DiscFields.Id), CancellationToken.None, true);

			// Assert

			AssertData(expectedFolder, receivedFolder);
		}

		[TestMethod]
		public async Task FolderQuery_ForUnknownFolder_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IFoldersQuery>();

			// Act

			var getSubfoldersTask = client.GetFolder(12345, RequestedFields, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getSubfoldersTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreateFolderMutation_ForCorrectInput_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData { Name = "Korn", ParentFolderId = 3, };

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData { Id = 8, Name = "Korn", },
				new OutputFolderData { Id = 4, Name = "Rammstein", },
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await foldersClient.GetFolder(3, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			// Assert

			AssertData(expectedFolders, receivedFolder.Subfolders);
			Assert.IsTrue(Directory.Exists(GetFullContentPath("Foreign/Korn")));
		}

		[TestMethod]
		public async Task CreateFolderMutation_IfFolderAlreadyExists_ReturnsError()
		{
			// Arrange

			var folderData = new InputFolderData { Name = "Rammstein", ParentFolderId = 3, };

			var client = CreateClient<IFoldersMutation>();

			// Act

			var createFolderTask = client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createFolderTask);
			Assert.AreEqual("Folder 'Rammstein' already exists", exception.Message);

			// Checking that no changes to the folders were made

			var expectedFolders = new[]
			{
				new OutputFolderData { Id = 4, Name = "Rammstein", },
			};

			var foldersClient = CreateClient<IFoldersQuery>();
			var receivedFolder = await foldersClient.GetFolder(3, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			AssertData(expectedFolders, receivedFolder.Subfolders);
		}

		[TestMethod]
		public async Task CreateFolderMutation_CreationOfFolderInStorageFails_DoesNotCreateFolderInRepository()
		{
			// Arrange

			var folderData = new InputFolderData { Name = "Korn", ParentFolderId = 3, };

			var storageServiceStub = new Mock<IStorageService>();
			storageServiceStub.Setup(x => x.CreateFolder(It.IsAny<Folder>(), It.IsAny<CancellationToken>()))
				.Throws(new ServiceOperationFailedException("Exception from IStorageService.CreateFolder()"));

			var client = CreateClient<IFoldersMutation>(services => services.AddSingleton<IStorageService>(storageServiceStub.Object));

			// Act

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => client.CreateFolder(folderData, CancellationToken.None));
			Assert.AreEqual("Exception from IStorageService.CreateFolder()", exception.Message);

			// Assert

			// Checking that no folders were created in the repository.

			var expectedFolders = new[]
			{
				new OutputFolderData { Id = 4, Name = "Rammstein", },
			};

			var foldersClient = CreateClient<IFoldersQuery>();
			var receivedFolder = await foldersClient.GetFolder(3, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			AssertData(expectedFolders, receivedFolder.Subfolders);
		}

		[TestMethod]
		public async Task CreateFolderMutation_CreationOfFolderInRepositoryFails_DoesNotCreateFolderInStorage()
		{
			// Arrange

			var folderData = new InputFolderData { Name = "Korn", ParentFolderId = 3, };

			var foldersRepositoryStub = new Mock<IFoldersRepository>();
			foldersRepositoryStub.Setup(x => x.GetFolder(3, It.IsAny<CancellationToken>())).ReturnsAsync(new Folder { Id = 3, ParentFolderId = 1, Name = "Foreign", });
			foldersRepositoryStub.Setup(x => x.GetFolder(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Folder { Id = 1, ParentFolderId = null, Name = "<ROOT>", });

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.Commit(It.IsAny<CancellationToken>()))
				.Throws(new ServiceOperationFailedException("Exception from IUnitOfWork.Commit()"));
			unitOfWorkStub.Setup(x => x.FoldersRepository).Returns(foldersRepositoryStub.Object);

			var client = CreateClient<IFoldersMutation>(services => services.AddSingleton<IUnitOfWork>(unitOfWorkStub.Object));

			// Act

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => client.CreateFolder(folderData, CancellationToken.None));
			Assert.AreEqual("Exception from IUnitOfWork.Commit()", exception.Message);

			// Assert

			// Sanity check, that we build paths correctly.
			Assert.IsTrue(Directory.Exists(GetFullContentPath("Foreign/Rammstein")));

			// Checking that no folders were created in the storage.
			Assert.IsFalse(Directory.Exists(GetFullContentPath("Foreign/Korn")));
		}

		[TestMethod]
		public async Task CreateFolderMutation_IfFolderExistsUnderAnotherParent_CreatesFolderSuccessfully()
		{
			// Arrange

			var folderData = new InputFolderData { Name = "Guano Apes", ParentFolderId = 2, };

			var client = CreateClient<IFoldersMutation>();

			// Act

			var newFolderId = await client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newFolderId);

			// Checking new folders data

			var expectedFolders = new[]
			{
				new OutputFolderData { Id = 8, Name = "Guano Apes", },
			};

			var foldersClient = CreateClient<IFoldersQuery>();

			// Act

			var receivedFolder = await foldersClient.GetFolder(2, FolderFields.Subfolders(FolderFields.Id + FolderFields.Name), CancellationToken.None);

			// Assert

			AssertData(expectedFolders, receivedFolder.Subfolders);
		}

		[TestMethod]
		public async Task CreateFolderMutation_IfParentFolderDoesNotExist_ReturnsError()
		{
			// Arrange

			var folderData = new InputFolderData { Name = "Some New Folder", ParentFolderId = 12345, };

			var client = CreateClient<IFoldersMutation>();

			// Act

			var createFolderTask = client.CreateFolder(folderData, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createFolderTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);
		}
	}
}
