using Microsoft.EntityFrameworkCore;

namespace MusicLibraryApi.IntegrationTests.Utility
{
	public class PostgreSqlIdentityInsert : IIdentityInsert
	{
		public void InitializeIdentityInsert(DbContext context, string tableName)
		{
		}

		public void FinalizeIdentityInsert(DbContext context, string tableName, int nextId)
		{
			context.Database.ExecuteSqlRaw($"ALTER SEQUENCE \"{tableName}_Id_seq\" RESTART WITH {nextId}");
		}
	}
}
