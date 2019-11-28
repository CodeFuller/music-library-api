using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MusicLibraryApi.Dal.EfCore
{
	internal static class DbUpdateExceptionExtensions
	{
		public static bool IsUniqueViolationException(this DbUpdateException exception)
		{
			return exception.InnerException is PostgresException pgException && pgException.SqlState == PostgresErrors.UniqueViolation;
		}
	}
}
