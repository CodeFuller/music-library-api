using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests
{
	public sealed partial class GraphQLTests
	{
		private class GenreDataComparer : IComparer
		{
			public int Compare(object? x, object? y)
			{
				var g1 = x as OutputGenreData;
				var g2 = y as OutputGenreData;

				if (Object.ReferenceEquals(g1, null) && Object.ReferenceEquals(g2, null))
				{
					return 0;
				}

				if (Object.ReferenceEquals(g1, null))
				{
					return -1;
				}

				if (Object.ReferenceEquals(g2, null))
				{
					return 1;
				}

				var cmp = Nullable.Compare(g1.Id, g2.Id);
				if (cmp != 0)
				{
					return cmp;
				}

				return String.Compare(g1.Name, g2.Name, StringComparison.Ordinal);
			}
		}

		[TestMethod]
		public async Task GenresQuery_ReturnsCorrectData()
		{
			// Arrange

			var expectedGenres = new[]
			{
				new OutputGenreData(1, "Russian Rock"),
				new OutputGenreData(2, "Nu Metal"),
				new OutputGenreData(3, "Alternative Rock"),
			};

			var client = CreateClient<IGenresQuery>();

			// Act

			var receivedGenres = await client.GetGenres(GenreFields.All, CancellationToken.None);

			// Assert

			CollectionAssert.AreEqual(expectedGenres, receivedGenres.ToList(), new GenreDataComparer());
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
				new OutputGenreData(1, "Russian Rock"),
				new OutputGenreData(2, "Nu Metal"),
				new OutputGenreData(3, "Alternative Rock"),
				new OutputGenreData(4, "Gothic Metal"),
			};

			var genresQuery = CreateClient<IGenresQuery>();
			var receivedGenres = await genresQuery.GetGenres(GenreFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedGenres, receivedGenres.ToList(), new GenreDataComparer());
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
				new OutputGenreData(1, "Russian Rock"),
				new OutputGenreData(2, "Nu Metal"),
				new OutputGenreData(3, "Alternative Rock"),
			};

			var genresQuery = CreateClient<IGenresQuery>();
			var receivedGenres = await genresQuery.GetGenres(GenreFields.All, CancellationToken.None);

			CollectionAssert.AreEqual(expectedGenres, receivedGenres.ToList(), new GenreDataComparer());
		}
	}
}
