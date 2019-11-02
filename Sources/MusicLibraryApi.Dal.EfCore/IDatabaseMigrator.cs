using Microsoft.EntityFrameworkCore;

namespace MusicLibraryApi.Dal.EfCore
{
	public interface IDatabaseMigrator
	{
		void Migrate(DbContext context);
	}
}
