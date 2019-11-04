using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContextFactory : IDesignTimeDbContextFactory<MusicLibraryDbContext>
	{
		public MusicLibraryDbContext CreateDbContext(string[] args)
		{
			var connectionString = LoadConnectionString();

			var optionsBuilder = new DbContextOptionsBuilder<MusicLibraryDbContext>();
			optionsBuilder.UseNpgsql(connectionString);

			return new MusicLibraryDbContext(optionsBuilder.Options);
		}

		private static string LoadConnectionString()
		{
			// Currently there is no proper way to configure connection string via tool arguments.
			// Track: https://github.com/aspnet/EntityFrameworkCore/issues/8332
			var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

			if (!File.Exists(appSettingsPath))
			{
				throw new InvalidOperationException($"File '{appSettingsPath}' is missing. The connection string for database migrations is read from this file.");
			}

			var configBuilder = new ConfigurationBuilder();
			configBuilder.AddJsonFile(appSettingsPath, optional: false);
			var configuration = configBuilder.Build();

			var connectionString = configuration.GetConnectionString("musicLibraryDB");

			Console.WriteLine($"Loaded connection string from '{appSettingsPath}': '{connectionString}'");

			return connectionString;
		}
	}
}
