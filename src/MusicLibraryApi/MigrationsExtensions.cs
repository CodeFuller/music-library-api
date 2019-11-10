using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApi.Dal.EfCore;

namespace MusicLibraryApi
{
	public static class MigrationsExtensions
	{
		public static IHost ApplyMigrations(this IHost host)
		{
			host.Services.ApplyMigrations();

			return host;
		}

		public static IServiceProvider ApplyMigrations(this IServiceProvider serviceProvider)
		{
			using var serviceScope = serviceProvider.CreateScope();
			var scopeServiceProvider = serviceScope.ServiceProvider;

			using var dbContext = scopeServiceProvider.GetRequiredService<MusicLibraryDbContext>();
			var dbMigrator = scopeServiceProvider.GetRequiredService<IDatabaseMigrator>();
			dbMigrator.Migrate(dbContext);

			return serviceProvider;
		}
	}
}
