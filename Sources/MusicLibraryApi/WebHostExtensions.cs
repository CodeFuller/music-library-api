using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApi.Dal.EfCore;

namespace MusicLibraryApi
{
	public static class WebHostExtensions
	{
		public static IHost ApplyMigrations(this IHost host)
		{
			var appServiceProvider = host.Services;
			using var serviceScope = appServiceProvider.CreateScope();
			var scopeServiceProvider = serviceScope.ServiceProvider;

			using var dbContext = scopeServiceProvider.GetRequiredService<MusicLibraryDbContext>();
			var dbMigrator = scopeServiceProvider.GetRequiredService<IDatabaseMigrator>();
			dbMigrator.Migrate(dbContext);

			return host;
		}
	}
}
