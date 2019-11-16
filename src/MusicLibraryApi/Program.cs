using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
