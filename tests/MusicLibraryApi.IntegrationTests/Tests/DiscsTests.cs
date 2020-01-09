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
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class DiscsTests : GraphQLTests
	{
		private static QueryFieldSet<OutputDiscData> RequestedFields => DiscFields.All + DiscFields.Folder(FolderFields.Id) + DiscFields.Songs(SongFields.Id);

		[TestMethod]
		public async Task DiscsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedDiscs = new[]
			{
				new OutputDiscData
				{
					Id = 1,
					Year = 2001,
					Title = "Platinum Hits (CD 2)",
					TreeTitle = "2001 - Platinum Hits (CD 2)",
					AlbumTitle = "Platinum Hits",
					AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
					AlbumOrder = 2,
					Folder = new OutputFolderData { Id = 1, },
					Songs = new[] { new OutputSongData { Id = 2, }, new OutputSongData { Id = 1, }, },
				},

				new OutputDiscData
				{
					Id = 2,
					Year = 2001,
					Title = "Platinum Hits (CD 1)",
					TreeTitle = "2001 - Platinum Hits (CD 1)",
					AlbumTitle = "Platinum Hits",
					AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
					AlbumOrder = 1,
					Folder = new OutputFolderData { Id = 1, },
					Songs = new[] { new OutputSongData { Id = 3, }, },
				},

				new OutputDiscData
				{
					Id = 3,
					Year = 2000,
					Title = "Don't Give Me Names",
					TreeTitle = "2000 - Don't Give Me Names",
					AlbumTitle = "Don't Give Me Names",
					Folder = new OutputFolderData { Id = 5, },
					Songs = Array.Empty<OutputSongData>(),
				},

				new OutputDiscData
				{
					Id = 4,
					Title = "Rarities",
					TreeTitle = "Rarities",
					AlbumTitle = String.Empty,
					Folder = new OutputFolderData { Id = 5, },
					Songs = Array.Empty<OutputSongData>(),
				},

				new OutputDiscData
				{
					Id = 5,
					Year = 1997,
					Title = "Proud Like A God",
					TreeTitle = "1997 - Proud Like A God",
					AlbumTitle = "Proud Like A God",
					Folder = new OutputFolderData { Id = 5, },
					Songs = Array.Empty<OutputSongData>(),
				},
			};

			var client = CreateClient<IDiscsQuery>();

			// Act

			var receivedDiscs = await client.GetDiscs(RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedDiscs, receivedDiscs);
		}

		[TestMethod]
		public async Task DiscQuery_ForExistingDisc_ReturnsCorrectData()
		{
			// Arrange

			var expectedDisc = new OutputDiscData
			{
				Id = 1,
				Year = 2001,
				Title = "Platinum Hits (CD 2)",
				TreeTitle = "2001 - Platinum Hits (CD 2)",
				AlbumTitle = "Platinum Hits",
				AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
				AlbumOrder = 2,
				Folder = new OutputFolderData { Id = 1, },
				Songs = new[] { new OutputSongData { Id = 2, }, new OutputSongData { Id = 1, }, },
			};

			var client = CreateClient<IDiscsQuery>();

			// Act

			var receivedDisc = await client.GetDisc(1, RequestedFields, CancellationToken.None);

			// Assert

			AssertData(expectedDisc, receivedDisc);
		}

		[TestMethod]
		public async Task DiscQuery_IfDiscDoesNotExist_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IDiscsQuery>();

			// Act

			var getDiscTask = client.GetDisc(12345, DiscFields.All, CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getDiscTask);
			Assert.AreEqual("The disc with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreateDiscMutation_ForDiscWithOptionalDataFilled_CreatesDiscSuccessfully()
		{
			// Arrange

			var newDiscData = new InputDiscData
			{
				FolderId = 5,
				Year = 1994,
				Title = "Битва на мотоциклах (CD 2)",
				TreeTitle = "1994 - Битва на мотоциклах (CD 2)",
				AlbumTitle = "Битва на мотоциклах",
				AlbumId = "{C7BEC024-8979-4477-8247-419A476C1DFB}",
				AlbumOrder = 2,
				DeleteDate = new DateTimeOffset(2019, 12, 10, 07, 20, 25, TimeSpan.FromHours(2)),
				DeleteComment = "For a test",
			};

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newDiscId);

			// Checking created disc data

			var expectedDisc = new OutputDiscData
			{
				Id = 8,
				Year = 1994,
				Title = "Битва на мотоциклах (CD 2)",
				TreeTitle = "1994 - Битва на мотоциклах (CD 2)",
				AlbumTitle = "Битва на мотоциклах",
				AlbumId = "{C7BEC024-8979-4477-8247-419A476C1DFB}",
				AlbumOrder = 2,
				DeleteDate = new DateTimeOffset(2019, 12, 10, 07, 20, 25, TimeSpan.FromHours(2)),
				DeleteComment = "For a test",
				Folder = new OutputFolderData { Id = 5, },
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDisc = await discsQuery.GetDisc(8, DiscFields.All + DiscFields.Folder(FolderFields.Id), CancellationToken.None);

			AssertData(expectedDisc, receivedDisc);
			Assert.IsTrue(Directory.Exists(GetFullContentPath("Guano Apes/1994 - Битва на мотоциклах (CD 2)")));
		}

		[TestMethod]
		public async Task CreateDiscMutation_ForDiscWithOptionalDataMissing_CreatesDiscSuccessfully()
		{
			// Arrange

			var newDiscData = new InputDiscData
			{
				FolderId = 5,
				Title = "Best Russian",
				TreeTitle = "Russian",
				AlbumTitle = String.Empty,
			};

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newDiscId);

			// Checking created disc data

			var expectedDisc = new OutputDiscData
			{
				Id = 8,
				Title = "Best Russian",
				TreeTitle = "Russian",
				AlbumTitle = String.Empty,
				Folder = new OutputFolderData { Id = 5, },
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDisc = await discsQuery.GetDisc(8, DiscFields.All + DiscFields.Folder(FolderFields.Id), CancellationToken.None);

			AssertData(expectedDisc, receivedDisc);
			Assert.IsTrue(Directory.Exists(GetFullContentPath("Guano Apes/Russian")));
		}

		[TestMethod]
		public async Task CreateDiscMutation_IfFolderDoesNotExist_ReturnsError()
		{
			// Arrange

			var newDiscData = new InputDiscData
			{
				FolderId = 12345,
				Title = "Some New Disc (CD 1)",
				TreeTitle = "1999 - Some New Disc (CD 1)",
				AlbumTitle = "Some New Disc",
			};

			var client = CreateClient<IDiscsMutation>();

			// Act

			var createDiscTask = client.CreateDisc(newDiscData, CancellationToken.None);

			// Arrange

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createDiscTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);

			// Checking that no disc was created

			var expectedDiscs = new[]
			{
				new OutputDiscData { Id = 1, },
				new OutputDiscData { Id = 2, },
				new OutputDiscData { Id = 3, },
				new OutputDiscData { Id = 4, },
				new OutputDiscData { Id = 5, },
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.Id, CancellationToken.None);

			AssertData(expectedDiscs, receivedDiscs);
		}

		[TestMethod]
		public async Task CreateDiscMutation_CreationOfDiscInStorageFails_DoesNotCreateDiscInRepository()
		{
			// Arrange

			var newDiscData = new InputDiscData
			{
				FolderId = 5,
				Title = "Some New Disc (CD 1)",
				TreeTitle = "1999 - Some New Disc (CD 1)",
				AlbumTitle = "Some New Disc",
			};

			var storageServiceStub = new Mock<IStorageService>();
			storageServiceStub.Setup(x => x.CreateDisc(It.IsAny<Disc>(), It.IsAny<CancellationToken>()))
				.Throws(new ServiceOperationFailedException("Exception from IStorageService.CreateDisc()"));

			var client = CreateClient<IDiscsMutation>(services => services.AddSingleton<IStorageService>(storageServiceStub.Object));

			// Act

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => client.CreateDisc(newDiscData, CancellationToken.None));
			Assert.AreEqual("Exception from IStorageService.CreateDisc()", exception.Message);

			// Arrange

			// Checking that no folders were created in the repository.

			var expectedDiscs = new[]
			{
				new OutputDiscData { Id = 1, },
				new OutputDiscData { Id = 2, },
				new OutputDiscData { Id = 3, },
				new OutputDiscData { Id = 4, },
				new OutputDiscData { Id = 5, },
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.Id, CancellationToken.None);

			AssertData(expectedDiscs, receivedDiscs);
		}

		[TestMethod]
		public async Task CreateDiscMutation_CreationOfDiscInRepositoryFails_DoesNotCreateDiscInStorage()
		{
			// Arrange

			var newDiscData = new InputDiscData
			{
				FolderId = 5,
				Title = "Some New Disc (CD 1)",
				TreeTitle = "1999 - Some New Disc (CD 1)",
				AlbumTitle = "Some New Disc",
			};

			var foldersRepositoryStub = new Mock<IFoldersRepository>();
			foldersRepositoryStub.Setup(x => x.GetFolder(5, It.IsAny<CancellationToken>())).ReturnsAsync(new Folder { Id = 5, ParentFolderId = 1, Name = "Guano Apes", });
			foldersRepositoryStub.Setup(x => x.GetFolder(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Folder { Id = 1, ParentFolderId = null, Name = "<ROOT>", });

			var discsRepositoryStub = new Mock<IDiscsRepository>();

			var unitOfWorkStub = new Mock<IUnitOfWork>();
			unitOfWorkStub.Setup(x => x.Commit(It.IsAny<CancellationToken>()))
				.Throws(new ServiceOperationFailedException("Exception from IUnitOfWork.Commit()"));
			unitOfWorkStub.Setup(x => x.FoldersRepository).Returns(foldersRepositoryStub.Object);
			unitOfWorkStub.Setup(x => x.DiscsRepository).Returns(discsRepositoryStub.Object);

			var client = CreateClient<IDiscsMutation>(services => services.AddSingleton<IUnitOfWork>(unitOfWorkStub.Object));

			// Act

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => client.CreateDisc(newDiscData, CancellationToken.None));
			Assert.AreEqual("Exception from IUnitOfWork.Commit()", exception.Message);

			// Arrange

			// Sanity check, that we build paths correctly.
			Assert.IsTrue(Directory.Exists(GetFullContentPath("Guano Apes/1997 - Proud Like A God")));

			// Checking that no folders were created in the storage.
			Assert.IsFalse(Directory.Exists(GetFullContentPath("Guano Apes/1999 - Some New Disc (CD 1)")));
		}
	}
}
