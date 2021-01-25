using System;
using System.Collections.Generic;
using System.Linq;
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
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
		private class ErrorItem
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
		{
			public string? Message { get; set; }
		}

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
		private class ErrorData
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
		{
			public IReadOnlyCollection<ErrorItem>? Errors { get; set; }
		}

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

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

			var receivedBody = await response.Content.ReadAsStringAsync();
			var errorData = JsonConvert.DeserializeObject<ErrorData>(receivedBody);

			Assert.AreEqual(1, errorData.Errors?.Count);
			Assert.AreEqual("Caught unhandled exception when processing the field \u0027error\u0027", errorData.Errors?.Single().Message);

			// Sanity check that stack trace does not present in any other response part.
			Assert.IsFalse(receivedBody.Contains(nameof(MusicLibraryApi), StringComparison.Ordinal));
		}
	}
}
