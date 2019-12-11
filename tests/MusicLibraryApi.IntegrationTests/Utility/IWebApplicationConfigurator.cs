using Microsoft.Extensions.DependencyInjection;

namespace MusicLibraryApi.IntegrationTests.Utility
{
	public interface IWebApplicationConfigurator
	{
		void ConfigureServices(IServiceCollection services);
	}
}
