using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicLibraryApi.IntegrationTests.Controllers
{
	public sealed partial class GraphQLControllerTests
	{
		[TestMethod]
		public async Task GenresQuery_ReturnsCorrectData()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await ExecuteGetGenresQuery(client);

			// Assert

			var expectedData = new
			{
				data = new
				{
					genres = new[]
					{
						new
						{
							id = 1,
							name = "Russian Rock",
						},
						new
						{
							id = 2,
							name = "Nu Metal",
						},
						new
						{
							id = 3,
							name = "Pop",
						},
					},
				},
			};

			await AssertResponse(response, expectedData);
		}

		[TestMethod]
		public async Task CreateGenreMutation_IfGenreDoesNotExist_CreatesGenreSuccessfully()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var requestBody = new
			{
				query = @"mutation ($genre: GenreInput!) {
							createGenre(genre: $genre) {
								newGenreId
							}
						}",

				variables = new
				{
					genre = new
					{
						name = "Gothic Metal",
					},
				},
			};

			var response = await client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);

			// Assert

			var expectedData = new
			{
				data = new
				{
					createGenre = new
					{
						newGenreId = 4,
					},
				},
			};

			await AssertResponse(response, expectedData);

			// Checking the genre data

			var expectedGenresData = new
			{
				data = new
				{
					genres = new[]
					{
						new
						{
							id = 1,
							name = "Russian Rock",
						},
						new
						{
							id = 2,
							name = "Nu Metal",
						},
						new
						{
							id = 3,
							name = "Pop",
						},
						new
						{
							id = 4,
							name = "Gothic Metal",
						},
					},
				},
			};

			var response2 = await ExecuteGetGenresQuery(client);
			await AssertResponse(response2, expectedGenresData);
		}

		private Task<HttpResponseMessage> ExecuteGetGenresQuery(HttpClient client)
		{
			var requestBody = new
			{
				query = @"{
							genres {
								id
								name
							}
						}",
			};

			return client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);
		}
	}
}
