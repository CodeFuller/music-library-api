using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

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
				new OutputDiscData(id: 1, year: 2001, title: "Platinum Hits (CD 2)", treeTitle: "2001 - Platinum Hits (CD 2)",
					albumTitle: "Platinum Hits", albumId: "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", albumOrder: 2, folder: new OutputFolderData(id: 1),
					songs: new[] { new OutputSongData(id: 2), new OutputSongData(id: 1), }),

				new OutputDiscData(id: 2, year: 2001, title: "Platinum Hits (CD 1)", treeTitle: "2001 - Platinum Hits (CD 1)",
					albumTitle: "Platinum Hits", albumId: "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", albumOrder: 1, folder: new OutputFolderData(id: 1),
					songs: new[] { new OutputSongData(id: 3), }),

				new OutputDiscData(id: 3, year: 2000, title: "Don't Give Me Names", treeTitle: "2000 - Don't Give Me Names",
					albumTitle: "Don't Give Me Names", folder: new OutputFolderData(id: 5),
					songs: Array.Empty<OutputSongData>()),

				new OutputDiscData(id: 4, title: "Rarities", treeTitle: "Rarities", albumTitle: String.Empty, folder: new OutputFolderData(id: 5), songs: Array.Empty<OutputSongData>()),

				new OutputDiscData(id: 5, year: 1997, "Proud Like A God", treeTitle: "1997 - Proud Like A God", albumTitle: "Proud Like A God",
					folder: new OutputFolderData(id: 5), songs: Array.Empty<OutputSongData>()),
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

			var expectedDisc = new OutputDiscData(id: 1, year: 2001, title: "Platinum Hits (CD 2)", treeTitle: "2001 - Platinum Hits (CD 2)",
				albumTitle: "Platinum Hits", albumId: "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", albumOrder: 2, folder: new OutputFolderData(id: 1),
				songs: new[] { new OutputSongData(id: 2), new OutputSongData(id: 1), });

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

			var newDiscData = new InputDiscData(5, 1994, "Битва на мотоциклах (CD 2)", "1994 - Битва на мотоциклах (CD 2)",
				"Битва на мотоциклах", "{C7BEC024-8979-4477-8247-419A476C1DFB}", 2, new DateTimeOffset(2019, 12, 10, 07, 20, 25, TimeSpan.FromHours(2)), "For a test");

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newDiscId);

			// Checking created disc data

			var expectedDisc = new OutputDiscData(id: 8, year: 1994, title: "Битва на мотоциклах (CD 2)", treeTitle: "1994 - Битва на мотоциклах (CD 2)",
				albumTitle: "Битва на мотоциклах", albumId: "{C7BEC024-8979-4477-8247-419A476C1DFB}", albumOrder: 2, folder: new OutputFolderData(id: 5),
				deleteDate: new DateTimeOffset(2019, 12, 10, 07, 20, 25, TimeSpan.FromHours(2)), deleteComment: "For a test");

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDisc = await discsQuery.GetDisc(8, DiscFields.All + DiscFields.Folder(FolderFields.Id), CancellationToken.None);

			AssertData(expectedDisc, receivedDisc);
		}

		[TestMethod]
		public async Task CreateDiscMutation_ForDiscWithOptionalDataMissing_CreatesDiscSuccessfully()
		{
			// Arrange

			var newDiscData = new InputDiscData(5, null, "Best Russian", "Russian", String.Empty);

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newDiscId);

			// Checking created disc data

			var expectedDisc = new OutputDiscData(id: 8, title: "Best Russian", treeTitle: "Russian", albumTitle: String.Empty, folder: new OutputFolderData(id: 5));

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDisc = await discsQuery.GetDisc(8, DiscFields.All + DiscFields.Folder(FolderFields.Id), CancellationToken.None);

			AssertData(expectedDisc, receivedDisc);
		}

		[TestMethod]
		public async Task CreateDiscMutation_IfFolderDoesNotExist_ReturnsError()
		{
			// Arrange

			var newDiscData = new InputDiscData(12345, null, "Some New Disc (CD 1)", "1999 - Some New Disc (CD 1)", "Some New Disc");

			var client = CreateClient<IDiscsMutation>();

			// Act

			var createDiscTask = client.CreateDisc(newDiscData, CancellationToken.None);

			// Arrange

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createDiscTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);

			// Checking that no disc was created

			var expectedDiscs = new[]
			{
				new OutputDiscData(id: 1),
				new OutputDiscData(id: 2),
				new OutputDiscData(id: 3),
				new OutputDiscData(id: 4),
				new OutputDiscData(id: 5),
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.Id, CancellationToken.None);

			AssertData(expectedDiscs, receivedDiscs);
		}
	}
}
