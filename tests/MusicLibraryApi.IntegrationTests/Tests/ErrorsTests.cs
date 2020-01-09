using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class ErrorsTests : GraphQLTests
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

			var client = CreateClient();

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

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

			var expectedBody = JsonConvert.SerializeObject(expectedData);
			var receivedBody = await response.Content.ReadAsStringAsync();

			Assert.AreEqual(expectedBody, receivedBody);
		}
	}
}
