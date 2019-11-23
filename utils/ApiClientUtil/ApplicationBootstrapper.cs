using CF.Library.Bootstrap;
using CF.Library.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client;

namespace ApiClientUtil
{
	public class ApplicationBootstrapper : DiApplicationBootstrapper<IApplicationLogic>
	{
		protected override void RegisterServices(IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ApiConnectionSettings>(options => configuration.Bind("apiConnection", options));
			services.AddMusicLibraryApiClient();

			services.AddTransient<IApplicationLogic, ApplicationLogic>();
		}

		protected override void BootstrapLogging(ILoggerFactory loggerFactory, IConfiguration configuration)
		{
			loggerFactory.LoadLoggingConfiguration(configuration);
		}
	}
}
