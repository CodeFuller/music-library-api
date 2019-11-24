﻿using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests
{
	public sealed partial class GraphQLTests
	{
		private class DiscDataComparer : IComparer
		{
			public int Compare(object? x, object? y)
			{
				var d1 = x as OutputDiscData;
				var d2 = y as OutputDiscData;

				if (Object.ReferenceEquals(d1, null) && Object.ReferenceEquals(d2, null))
				{
					return 0;
				}

				if (Object.ReferenceEquals(d1, null))
				{
					return -1;
				}

				if (Object.ReferenceEquals(d2, null))
				{
					return 1;
				}

				var cmp = Nullable.Compare(d1.Id, d2.Id);
				if (cmp != 0)
				{
					return cmp;
				}

				cmp = Nullable.Compare(d1.Year, d2.Year);
				if (cmp != 0)
				{
					return cmp;
				}

				cmp = String.Compare(d1.Title, d2.Title, StringComparison.Ordinal);
				if (cmp != 0)
				{
					return cmp;
				}

				cmp = String.Compare(d1.AlbumTitle, d2.AlbumTitle, StringComparison.Ordinal);
				if (cmp != 0)
				{
					return cmp;
				}

				cmp = Nullable.Compare(d1.AlbumOrder, d2.AlbumOrder);
				if (cmp != 0)
				{
					return cmp;
				}

				cmp = Nullable.Compare(d1.DeleteDate, d2.DeleteDate);
				if (cmp != 0)
				{
					return cmp;
				}

				return String.Compare(d1.DeleteComment, d2.DeleteComment, StringComparison.Ordinal);
			}
		}

		[TestMethod]
		public async Task DiscsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedDiscs = new[]
			{
				new OutputDiscData(1, 1988, "Князь тишины", null, null, null, null),
				new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1, new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(3, 2001, "Platinum Hits (CD 2)", "Platinum Hits", 2, new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(4, null, "Foreign", null, null, null, null),
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

			var expectedData = new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1, new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)), "Boring");

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

			var expectedData = new OutputDiscData(4, null, "Foreign", null, null, null, null);

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

			var newDiscData = new InputDiscData(1994, "Битва на мотоциклах (CD 2)", "Битва на мотоциклах", 2, new DateTimeOffset(2019, 11, 10, 18, 50, 24, TimeSpan.FromHours(2)), "Deleted just for test :)");

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(4, newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newDiscId);

			// Checking new discs data

			var expectedDiscs = new[]
			{
				new OutputDiscData(1, 1988, "Князь тишины", null, null, null, null),
				new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1, new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(3, 2001, "Platinum Hits (CD 2)", "Platinum Hits", 2, new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(4, null, "Foreign", null, null, null, null),
				new OutputDiscData(5, 1994, "Битва на мотоциклах (CD 2)", "Битва на мотоциклах", 2, new DateTimeOffset(2019, 11, 10, 18, 50, 24, TimeSpan.FromHours(2)), "Deleted just for test :)"),
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedDiscs, receivedDiscs.ToList(), new DiscDataComparer());
		}

		[TestMethod]
		public async Task CreateDiscMutation_ForDiscWithOptionalDataMissing_CreatesDiscSuccessfully()
		{
			// Arrange

			var newDiscData = new InputDiscData(null, "Russian", null, null, null, null);

			var client = CreateClient<IDiscsMutation>();

			// Act

			var newDiscId = await client.CreateDisc(4, newDiscData, CancellationToken.None);

			// Assert

			Assert.AreEqual(5, newDiscId);

			// Checking new discs data

			var expectedDiscs = new[]
			{
				new OutputDiscData(1, 1988, "Князь тишины", null, null, null, null),
				new OutputDiscData(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1, new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(3, 2001, "Platinum Hits (CD 2)", "Platinum Hits", 2, new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)), "Boring"),
				new OutputDiscData(4, null, "Foreign", null, null, null, null),
				new OutputDiscData(5, null, "Russian", null, null, null, null),
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedDiscs, receivedDiscs.ToList(), new DiscDataComparer());
		}

		[TestMethod]
		public async Task CreateDiscMutation_IfFolderDoesNotExist_ReturnsError()
		{
			// Arrange

			var newDiscData = new InputDiscData(null, "Some New Disc", null, null, null, null);

			var client = CreateClient<IDiscsMutation>();

			// Act

			var createDiscTask = client.CreateDisc(12345, newDiscData, CancellationToken.None);

			// Arrange

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createDiscTask);
			Assert.AreEqual("The folder with id of '12345' does not exist", exception.Message);

			// Checking that no changes to the discs were made

			var expectedDiscs = new[]
			{
				new OutputDiscData(1, null, null, null, null, null, null),
				new OutputDiscData(2, null, null, null, null, null, null),
				new OutputDiscData(3, null, null, null, null, null, null),
				new OutputDiscData(4, null, null, null, null, null, null),
			};

			var discsQuery = CreateClient<IDiscsQuery>();
			var receivedDiscs = await discsQuery.GetDiscs(DiscFields.Id, CancellationToken.None);

			CollectionAssert.AreEqual(expectedDiscs, receivedDiscs.ToList(), new GenreDataComparer());
		}
	}
}
