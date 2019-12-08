using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.IntegrationTests.Comparers;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class DiscsTests : GraphQLTests
	{
		[TestMethod]
		public async Task DiscsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedDiscs = new[]
			{
				new OutputDiscData(1, 2001, "Platinum Hits (CD 2)", "2001 - Platinum Hits (CD 2)", "Platinum Hits", "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 2),
				new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "2001 - Platinum Hits (CD 1)", "Platinum Hits", "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 1),
				new OutputDiscData(3, 2000, "Don't Give Me Names", "2000 - Don't Give Me Names", "Don't Give Me Names"),
				new OutputDiscData(4, null, "Rarities", "Rarities", String.Empty),
				new OutputDiscData(5, 1997, "Proud Like A God", "1997 - Proud Like A God", "Proud Like A God"),
			};

			var client = CreateClient<IDiscsQuery>();

			// Act

			var receivedDiscs = await client.GetDiscs(DiscFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedDiscs, receivedDiscs.ToList(), new DiscDataComparer());
		}

		[TestMethod]
		public async Task DiscQuery_ForDiscWithOptionalDataFilled_ReturnsCorrectData()
		{
			// Arrange

			var expectedData = new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "2001 - Platinum Hits (CD 1)", "Platinum Hits", "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 1);

			var client = CreateClient<IDiscsQuery>();

			// Act

			var receivedData = await client.GetDisc(2, DiscFields.All, CancellationToken.None);

			// Assert

			var cmp = new DiscDataComparer().Compare(expectedData, receivedData);
			Assert.AreEqual(0, cmp, "Discs data does not match");
		}

		[TestMethod]
		public async Task DiscQuery_ForDiscWithOptionalDataMissing_ReturnsCorrectData()
		{
			// Arrange

			var expectedData = new OutputDiscData(4, null, "Rarities", "Rarities", String.Empty);

			var client = CreateClient<IDiscsQuery>();

			// Act

			var receivedData = await client.GetDisc(4, DiscFields.All, CancellationToken.None);

			// Assert

			var cmp = new DiscDataComparer().Compare(expectedData, receivedData);
			Assert.AreEqual(0, cmp, "Discs data does not match");
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

			var newDiscData = new InputDiscData(5, 1994, "Битва на мотоциклах (CD 2)", "1994 - Битва на мотоциклах (CD 2)", "Битва на мотоциклах", "{C7BEC024-8979-4477-8247-419A476C1DFB}", 2);

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(8, newDiscId);

			// Checking new discs data

			var expectedDiscs = new[]
			{
				new OutputDiscData(8, 1994, "Битва на мотоциклах (CD 2)", "1994 - Битва на мотоциклах (CD 2)", "Битва на мотоциклах", "{C7BEC024-8979-4477-8247-419A476C1DFB}", 2),
				new OutputDiscData(5, 1997, "Proud Like A God", "1997 - Proud Like A God", "Proud Like A God"),
				new OutputDiscData(3, 2000, "Don't Give Me Names", "2000 - Don't Give Me Names", "Don't Give Me Names"),
				new OutputDiscData(4, null, "Rarities", "Rarities", String.Empty),
			};

			var foldersQuery = CreateClient<IFoldersQuery>();
			var folderData = await foldersQuery.GetFolder(5, FolderFields.Discs(DiscFields.All), CancellationToken.None);

			CollectionAssert.AreEqual(expectedDiscs, folderData.Discs.ToList(), new DiscDataComparer());
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

			// Checking new discs data

			var expectedDiscs = new[]
			{
				new OutputDiscData(5, 1997, "Proud Like A God", "1997 - Proud Like A God", "Proud Like A God"),
				new OutputDiscData(3, 2000, "Don't Give Me Names", "2000 - Don't Give Me Names", "Don't Give Me Names"),
				new OutputDiscData(4, null, "Rarities", "Rarities", String.Empty),
				new OutputDiscData(8, null, "Best Russian", "Russian", String.Empty),
			};

			var foldersQuery = CreateClient<IFoldersQuery>();
			var folderData = await foldersQuery.GetFolder(5, FolderFields.Discs(DiscFields.All), CancellationToken.None);

			CollectionAssert.AreEqual(expectedDiscs, folderData.Discs.ToList(), new DiscDataComparer());
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
				new OutputDiscData(1, null, null, null, null),
				new OutputDiscData(2, null, null, null, null),
				new OutputDiscData(3, null, null, null, null),
				new OutputDiscData(4, null, null, null, null),
				new OutputDiscData(5, null, null, null, null),
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedDiscs, receivedDiscs.ToList(), new DiscDataComparer());
		}
	}
}
