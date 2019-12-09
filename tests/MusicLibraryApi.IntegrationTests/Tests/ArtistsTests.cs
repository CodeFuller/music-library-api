using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class ArtistsTests : GraphQLTests
	{
		[TestMethod]
		public async Task ArtistsQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedArtists = new[]
			{
				new OutputArtistData(2, "AC/DC", new[] { new OutputSongData(id: 2), }),
				new OutputArtistData(1, "Nautilus Pompilius", new[] { new OutputSongData(id: 3), }),
				new OutputArtistData(3, "Кино", Array.Empty<OutputSongData>()),
			};

			var client = CreateClient<IArtistsQuery>();

			// Act

			var receivedArtists = await client.GetArtists(ArtistFields.All + ArtistFields.Songs(SongFields.Id), CancellationToken.None);

			// Assert

			AssertData(expectedArtists, receivedArtists);
		}

		[TestMethod]
		public async Task ArtistQuery_ForExistingArtist_ReturnsCorrectData()
		{
			// Arrange

			var expectedArtist = new OutputArtistData(2, "AC/DC", new[] { new OutputSongData(id: 2), });

			var client = CreateClient<IArtistsQuery>();

			// Act

			var receivedArtist = await client.GetArtist(2, ArtistFields.All + ArtistFields.Songs(SongFields.Id), CancellationToken.None);

			// Assert

			AssertData(expectedArtist, receivedArtist);
		}

		[TestMethod]
		public async Task ArtistQuery_IfArtistDoesNotExist_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IArtistsQuery>();

			// Act

			var getDiscTask = client.GetArtist(12345, ArtistFields.All + ArtistFields.Songs(SongFields.Id), CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getDiscTask);
			Assert.AreEqual("The artist with id of '12345' does not exist", exception.Message);
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

			AssertData(expectedArtists, receivedArtists);
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

			AssertData(expectedArtists, receivedArtists);
		}
	}
}
