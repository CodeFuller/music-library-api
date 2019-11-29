using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MusicLibraryApi.IntegrationTests
{
	public abstract class GraphQLTests : IDisposable
	{
		protected CustomWebApplicationFactory WebApplicationFactory { get; } = new CustomWebApplicationFactory();

		[TestInitialize]
		public void Initialize()
		{
			WebApplicationFactory.SeedData();
		}

		protected TClient CreateClient<TClient>()
		{
			return WebApplicationFactory.Services.GetRequiredService<TClient>();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				WebApplicationFactory?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
