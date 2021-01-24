using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MusicLibraryApi
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			await CreateHostBuilder(args)
				.Build()
				.ApplyMigrations()
				.RunAsync()
				.ConfigureAwait(false);
		}

		// The method CreateHostBuilder() is also used by EF Core Tools (i.e. dotnet ef migrations).
		// See the following article for more details: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.AddJsonFile("appsettings.json", optional: false)
						.AddEnvironmentVariables()
						.AddCommandLine(args);
				})
				.ConfigureLogging((context, logging) =>
				{
					// Disabling any built-in logging
					// Custom logging is configured in Startup.Configure().
					logging.ClearProviders();

					// Setting native logging level to most verbose.
					// Actual logging level will be set by CodeFuller.Library.Logging based on settings in Logging section.
					logging.SetMinimumLevel(LogLevel.Trace);
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
