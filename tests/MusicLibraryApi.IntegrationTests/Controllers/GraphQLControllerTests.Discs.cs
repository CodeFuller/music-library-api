using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.FormattableString;

namespace MusicLibraryApi.IntegrationTests.Controllers
{
	public sealed partial class GraphQLControllerTests
	{
		[TestMethod]
		public async Task DiscsQuery_ReturnsCorrectData()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await GetDiscsData(client);

			// Assert

			var expectedData = new
			{
				data = new
				{
					discs = new[]
					{
						new
						{
							id = 1,
							year = (int?)1988,
							title = "Князь тишины",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},

						new
						{
							id = 2,
							year = (int?)2001,
							title = "Platinum Hits (CD 1)",
							albumTitle = (string?)"Platinum Hits",
							albumOrder = (int?)1,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Boring",
						},

						new
						{
							id = 3,
							year = (int?)2001,
							title = "Platinum Hits (CD 2)",
							albumTitle = (string?)"Platinum Hits",
							albumOrder = (int?)2,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Boring",
						},

						new
						{
							id = 4,
							year = (int?)null,
							title = "Foreign",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},
					},
				},
			};

			await AssertResponse(response, expectedData);
		}

		[TestMethod]
		public async Task DiscQuery_ForDiscWithOptionalDataFilled_ReturnsCorrectData()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await ExecuteGetDiscQuery(client, 2);

			// Assert

			var expectedData = new
			{
				data = new
				{
					disc = new
					{
						id = 2,
						year = 2001,
						title = "Platinum Hits (CD 1)",
						albumTitle = "Platinum Hits",
						albumOrder = 1,
						deleteDate = new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)).ToUniversalTime(),
						deleteComment = "Boring",
					},
				},
			};

			await AssertResponse(response, expectedData);
		}

		[TestMethod]
		public async Task DiscQuery_ForDiscWithOptionalDataMissing_ReturnsCorrectData()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await ExecuteGetDiscQuery(client, 4);

			// Assert

			var expectedData = new
			{
				data = new
				{
					disc = new
					{
						id = 4,
						year = (int?)null,
						title = "Foreign",
						albumTitle = (string?)null,
						albumOrder = (int?)null,
						deleteDate = (DateTimeOffset?)null,
						deleteComment = (string?)null,
					},
				},
			};

			await AssertResponse(response, expectedData);
		}

		[TestMethod]
		public async Task DiscQuery_IfDiscDoesNotExist_ReturnsError()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await ExecuteGetDiscQuery(client, 12345);

			// Assert

			var expectedData = new
			{
				data = new
				{
					disc = (object?)null,
				},

				errors = new[]
				{
					new
					{
						message = "The disc with id of '12345' does not exist",
						locations = new[]
						{
							new
							{
								line = 2,
								column = 8,
							},
						},
						path = new[] { "disc" },
					},
				},
			};

			await AssertResponse(response, expectedData);
		}

		[TestMethod]
		public async Task CreateDiscMutation_ForDiscWithOptionalDataFilled_CreatesDiscSuccessfully()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			var discData = new
			{
				year = 1994,
				title = "Битва на мотоциклах (CD 2)",
				albumTitle = "Битва на мотоциклах",
				albumOrder = 2,
				deleteDate = new DateTimeOffset(2019, 11, 10, 18, 50, 24, TimeSpan.FromHours(2)),
				deleteComment = "Deleted just for test :)",
			};

			// Act

			var response = await ExecuteCreateDiscMutation(client, discData, 4);

			// Assert

			var expectedData = new
			{
				data = new
				{
					createDisc = new
					{
						newDiscId = 5,
					},
				},
			};

			await AssertResponse(response, expectedData);

			// Checking the genre data

			var expectedDiscsData = new
			{
				data = new
				{
					discs = new[]
					{
						new
						{
							id = 1,
							year = (int?)1988,
							title = "Князь тишины",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},

						new
						{
							id = 2,
							year = (int?)2001,
							title = "Platinum Hits (CD 1)",
							albumTitle = (string?)"Platinum Hits",
							albumOrder = (int?)1,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Boring",
						},

						new
						{
							id = 3,
							year = (int?)2001,
							title = "Platinum Hits (CD 2)",
							albumTitle = (string?)"Platinum Hits",
							albumOrder = (int?)2,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Boring",
						},

						new
						{
							id = 4,
							year = (int?)null,
							title = "Foreign",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},

						new
						{
							id = 5,
							year = (int?)1994,
							title = "Битва на мотоциклах (CD 2)",
							albumTitle = (string?)"Битва на мотоциклах",
							albumOrder = (int?)2,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 18, 50, 24, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Deleted just for test :)",
						},
					},
				},
			};

			var response2 = await GetDiscsData(client);
			await AssertResponse(response2, expectedDiscsData);
		}

		[TestMethod]
		public async Task CreateDiscMutation_ForDiscWithOptionalDataMissing_CreatesDiscSuccessfully()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			var discData = new
			{
				title = "Russian",
			};

			// Act

			var response = await ExecuteCreateDiscMutation(client, discData, 4);

			// Assert

			var expectedData = new
			{
				data = new
				{
					createDisc = new
					{
						newDiscId = 5,
					},
				},
			};

			await AssertResponse(response, expectedData);

			// Checking the genre data

			var expectedDiscsData = new
			{
				data = new
				{
					discs = new[]
					{
						new
						{
							id = 1,
							year = (int?)1988,
							title = "Князь тишины",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},

						new
						{
							id = 2,
							year = (int?)2001,
							title = "Platinum Hits (CD 1)",
							albumTitle = (string?)"Platinum Hits",
							albumOrder = (int?)1,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Boring",
						},

						new
						{
							id = 3,
							year = (int?)2001,
							title = "Platinum Hits (CD 2)",
							albumTitle = (string?)"Platinum Hits",
							albumOrder = (int?)2,
							deleteDate = (DateTimeOffset?)new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)).ToUniversalTime(),
							deleteComment = (string?)"Boring",
						},

						new
						{
							id = 4,
							year = (int?)null,
							title = "Foreign",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},

						new
						{
							id = 5,
							year = (int?)null,
							title = "Russian",
							albumTitle = (string?)null,
							albumOrder = (int?)null,
							deleteDate = (DateTimeOffset?)null,
							deleteComment = (string?)null,
						},
					},
				},
			};

			var response2 = await GetDiscsData(client);
			await AssertResponse(response2, expectedDiscsData);
		}

		[TestMethod]
		public async Task CreateDiscMutation_IfFolderDoesNotExist_ReturnsError()
		{
			// Arrange

			var client = webApplicationFactory.CreateClient();

			var discData = new
			{
				title = "Some New Disc",
			};

			// Act

			var response = await ExecuteCreateDiscMutation(client, discData, 12345);

			// Assert

			var expectedData = new
			{
				data = new
				{
					createDisc = (object?)null,
				},

				errors = new[]
				{
					new
					{
						message = "The folder with id of '12345' does not exist",
						locations = new[]
						{
							new
							{
								line = 2,
								column = 8,
							},
						},
						path = new[] { "createDisc" },
					},
				},
			};

			await AssertResponse(response, expectedData);

			// Checking the genre data

			var expectedDiscsData = new
			{
				data = new
				{
					discs = new[]
					{
						new { id = 1, },
						new { id = 2, },
						new { id = 3, },
						new { id = 4, },
					},
				},
			};

			var response2 = await GetDiscsIds(client);
			await AssertResponse(response2, expectedDiscsData);
		}

		private Task<HttpResponseMessage> GetDiscsData(HttpClient client)
		{
			var requestBody = new
			{
				query = @"{
							discs {
								id
								year
								title
								albumTitle
								albumOrder
								deleteDate
								deleteComment
							}
						}",
			};

			return client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);
		}

		private Task<HttpResponseMessage> GetDiscsIds(HttpClient client)
		{
			var requestBody = new
			{
				query = @"{
							discs {
								id
							}
						}",
			};

			return client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);
		}

		private Task<HttpResponseMessage> ExecuteGetDiscQuery(HttpClient client, int discId)
		{
			var requestBody = new
			{
				query = Invariant($@"{{
							disc(id: {discId}) {{
								id
								year
								title
								albumTitle
								albumOrder
								deleteDate
								deleteComment
							}}
						}}"),
			};

			return client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);
		}

		private Task<HttpResponseMessage> ExecuteCreateDiscMutation(HttpClient client, object discData, int folderId)
		{
			var requestBody = new
			{
				query = @"mutation ($disc: DiscInput!, $folderId: ID!) {
							createDisc(disc: $disc, folderId: $folderId) {
								newDiscId
							}
						}",

				variables = new
				{
					disc = discData,
					folderId = folderId,
				},
			};

			return client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);
		}
	}
}
