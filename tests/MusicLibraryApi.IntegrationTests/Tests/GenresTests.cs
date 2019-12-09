using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class GenresTests : GraphQLTests
	{
		[TestMethod]
		public async Task GenresQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedGenres = new[]
			{
				new OutputGenreData(3, "Alternative Rock", Array.Empty<OutputSongData>()),
				new OutputGenreData(2, "Nu Metal", new[] { new OutputSongData(id: 1), }),
				new OutputGenreData(1, "Russian Rock", new[] { new OutputSongData(id: 2), }),
			};

			var client = CreateClient<IGenresQuery>();

			// Act

			var receivedGenres = await client.GetGenres(GenreFields.All + GenreFields.Songs(SongFields.Id), CancellationToken.None);

			// Assert

			AssertData(expectedGenres, receivedGenres);
		}

		[TestMethod]
		public async Task GenreQuery_ForExistingGenre_ReturnsCorrectData()
		{
			// Arrange

			var expectedGenre = new OutputGenreData(2, "Nu Metal", new[] { new OutputSongData(id: 1), });

			var client = CreateClient<IGenresQuery>();

			// Act

			var receivedGenre = await client.GetGenre(2, GenreFields.All + GenreFields.Songs(SongFields.Id), CancellationToken.None);

			// Assert

			AssertData(expectedGenre, receivedGenre);
		}

		[TestMethod]
		public async Task GenreQuery_IfGenreDoesNotExist_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IGenresQuery>();

			// Act

			var getDiscTask = client.GetGenre(12345, GenreFields.All + GenreFields.Songs(SongFields.Id), CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => getDiscTask);
			Assert.AreEqual("The genre with id of '12345' does not exist", exception.Message);
		}

		[TestMethod]
		public async Task CreateGenreMutation_IfGenreDoesNotExist_CreatesGenreSuccessfully()
		{
			// Arrange

			var client = CreateClient<IGenresMutation>();

			// Act

			var newGenreId = await client.CreateGenre(new InputGenreData("Gothic Metal"), CancellationToken.None);

			// Assert

			Assert.AreEqual(4, newGenreId);

			// Checking new genres data

			var expectedGenres = new[]
			{
				new OutputGenreData(3, "Alternative Rock"),
				new OutputGenreData(4, "Gothic Metal"),
				new OutputGenreData(2, "Nu Metal"),
				new OutputGenreData(1, "Russian Rock"),
			};

			var genresQuery = CreateClient<IGenresQuery>();
			var receivedGenres = await genresQuery.GetGenres(GenreFields.All, CancellationToken.None);

			AssertData(expectedGenres, receivedGenres);
		}

		[TestMethod]
		public async Task CreateGenreMutation_IfGenreExists_ReturnsError()
		{
			// Arrange

			var client = CreateClient<IGenresMutation>();

			// Act

			var createGenreTask = client.CreateGenre(new InputGenreData("Nu Metal"), CancellationToken.None);

			// Assert

			var exception = await Assert.ThrowsExceptionAsync<GraphQLRequestFailedException>(() => createGenreTask);
			Assert.AreEqual("Genre 'Nu Metal' already exists", exception.Message);

			// Checking that no changes to the genres were made

			var expectedGenres = new[]
			{
				new OutputGenreData(3, "Alternative Rock"),
				new OutputGenreData(2, "Nu Metal"),
				new OutputGenreData(1, "Russian Rock"),
			};

			var genresQuery = CreateClient<IGenresQuery>();
			var receivedGenres = await genresQuery.GetGenres(GenreFields.All, CancellationToken.None);

			AssertData(expectedGenres, receivedGenres);
		}
	}
}
