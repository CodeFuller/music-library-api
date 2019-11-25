﻿using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests
{
	public sealed partial class GraphQLTests
	{
		private class ArtistDataComparer : IComparer
		{
			public int Compare(object? x, object? y)
			{
				// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
				var a1 = (OutputArtistData?)x;
				var a2 = (OutputArtistData?)y;

				if (Object.ReferenceEquals(a1, null) && Object.ReferenceEquals(a2, null))
				{
					return 0;
				}

				if (Object.ReferenceEquals(a1, null))
				{
					return -1;
				}

				if (Object.ReferenceEquals(a2, null))
				{
					return 1;
				}

				var cmp = Nullable.Compare(a1.Id, a2.Id);
				if (cmp != 0)
				{
					return cmp;
				}

				return String.Compare(a1.Name, a2.Name, StringComparison.Ordinal);
			}
		}

		[TestMethod]
		public async Task ArtistsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedArtists = new[]
			{
				new OutputArtistData(2, "AC/DC"),
				new OutputArtistData(1, "Nautilus Pompilius"),
				new OutputArtistData(3, "Кино"),
			};

			var client = CreateClient<IArtistsQuery>();

			// Act

			var receivedArtists = await client.GetArtists(ArtistFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedArtists, receivedArtists.ToList(), new ArtistDataComparer());
		}

		[TestMethod]
		public async Task CreateArtistMutation_IfArtistDoesNotExist_CreatesArtistSuccessfully()
		{
			// Arrange

			var client = CreateClient<IArtistsMutation>();

			// Act

			var newArtistId = await client.CreateArtist(new InputArtistData("Агата Кристи"), CancellationToken.None);

			// Assert

			Assert.AreEqual(4, newArtistId);

			// Checking new artists data

			var expectedArtists = new[]
			{
				new OutputArtistData(2, "AC/DC"),
				new OutputArtistData(1, "Nautilus Pompilius"),
				new OutputArtistData(4, "Агата Кристи"),
				new OutputArtistData(3, "Кино"),
			};

			var artistsQuery = CreateClient<IArtistsQuery>();
			var receivedArtists = await artistsQuery.GetArtists(ArtistFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedArtists, receivedArtists.ToList(), new ArtistDataComparer());
		}

		[TestMethod]
		public async Task CreateArtistMutation_IfArtistExists_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IArtistsMutation>();

			// Act

			var createArtistTask = client.CreateArtist(new InputArtistData("AC/DC"), CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createArtistTask);
			Assert.AreEqual("Artist 'AC/DC' already exists", exception.Message);

			// Checking that no changes to the artists were made

			var expectedArtists = new[]
			{
				new OutputArtistData(2, "AC/DC"),
				new OutputArtistData(1, "Nautilus Pompilius"),
				new OutputArtistData(3, "Кино"),
			};

			var artistsQuery = CreateClient<IArtistsQuery>();
			var receivedArtists = await artistsQuery.GetArtists(ArtistFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedArtists, receivedArtists.ToList(), new ArtistDataComparer());
		}
	}
}
