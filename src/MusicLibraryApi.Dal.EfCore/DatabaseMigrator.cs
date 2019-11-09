using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MusicLibraryApi.Dal.EfCore
{
	public class DatabaseMigrator : IDatabaseMigrator
	{
		private readonly ILogger<DatabaseMigrator> logger;

		public DatabaseMigrator(ILogger<DatabaseMigrator> logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void Migrate(DbContext context)
		{
			logger.LogInformation("Migrating the database...");
			context.Database.Migrate();
			logger.LogInformation("The database was migrated successfully");
		}
	}
}
