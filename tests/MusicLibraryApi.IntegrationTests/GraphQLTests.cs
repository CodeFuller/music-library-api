using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

		private TClient CreateClient<TClient>()
		{
			return webApplicationFactory.Services.GetRequiredService<TClient>();
		}

		public void Dispose()
		{
			webApplicationFactory?.Dispose();
		}
	}
}
