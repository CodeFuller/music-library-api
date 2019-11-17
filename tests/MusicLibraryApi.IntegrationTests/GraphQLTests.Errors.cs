using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicLibraryApi.IntegrationTests
{
	public sealed partial class GraphQLTests
	{
		[TestMethod]
		public async Task ErrorsQuery_DoesNotExposeInternalSensitiveError()
		{
			// Arrange

			var requestBody = new
			{
				query = @"{
							error
						}",
			};

			var client = webApplicationFactory.CreateClient();

			// Act

			var response = await client.PostAsJsonAsync(new Uri("/graphql", UriKind.Relative), requestBody);

			// Assert

			var expectedData = new
			{
				data = new
				{
					error = (object?)null,
				},

				errors = new[]
				{
					new
					{
						message = "Caught unhandled exception when processing the field 'error'",
						locations = new[]
						{
							new
							{
								line = 2,
								column = 8,
							},
						},
						path = new[] { "error" },
					},
				},
			};

			await AssertResponse(response, expectedData);
		}
	}
}
