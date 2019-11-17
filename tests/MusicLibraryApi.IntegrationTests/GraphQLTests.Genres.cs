using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicLibraryApi.IntegrationTests
{
	public sealed partial class GraphQLTests
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
							name = "Alternative Rock",
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

			var response = await ExecuteCreateGenreMutation(client, "Gothic Metal");

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
							name = "Alternative Rock",
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

		[TestMethod]
		public async Task CreateGenreMutation_IfGenreExists_ReturnsError()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await ExecuteCreateGenreMutation(client, "Nu Metal");

			// Assert

			var expectedData = new
			{
				data = new
				{
					createGenre = (object?)null,
				},

				errors = new[]
				{
					new
					{
						message = "Genre 'Nu Metal' already exists",
						locations = new[]
						{
							new
							{
								line = 2,
								column = 8,
							},
						},
						path = new[] { "createGenre" },
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
							name = "Alternative Rock",
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

		private Task<HttpResponseMessage> ExecuteCreateGenreMutation(HttpClient client, object genreName)
		{
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
						name = genreName,
					},
				},
			};

			return client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);
		}
	}
}
