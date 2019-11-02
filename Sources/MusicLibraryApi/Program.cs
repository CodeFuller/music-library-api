using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MusicLibraryApi
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			await CreateHostBuilder(args)
				.Build()
				.ApplyMigrations()
				.RunAsync()
				.ConfigureAwait(false);
		}

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
