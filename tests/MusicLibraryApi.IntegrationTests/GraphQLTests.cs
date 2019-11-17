using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MusicLibraryApi.IntegrationTests
{
	[TestClass]
	public sealed partial class GraphQLTests : IDisposable
	{
		private readonly CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();

		[TestInitialize]
		public void Initialize()
		{
			webApplicationFactory.SeedData();
		}

		public void Dispose()
		{
			webApplicationFactory?.Dispose();
		}

		private static async Task AssertResponse(HttpResponseMessage response, object expectedData)
		{
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

			var expectedBody = JsonConvert.SerializeObject(expectedData);
			var receivedBody = await response.Content.ReadAsStringAsync();

			Assert.AreEqual(expectedBody, receivedBody);
		}
	}
}
