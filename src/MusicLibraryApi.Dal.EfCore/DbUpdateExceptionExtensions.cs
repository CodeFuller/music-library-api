using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MusicLibraryApi.Dal.EfCore
{
	internal static class DbUpdateExceptionExtensions
	{
		public static bool IsUniqueViolationException(this DbUpdateException exception)
		{
			return IsPostgresException(exception, PostgresErrors.UniqueViolation, out _);
		}

		public static bool IsForeignKeyViolationException(this DbUpdateException exception, out string? constraintName)
		{
			return IsPostgresException(exception, PostgresErrors.ForeignKeyViolation, out constraintName);
		}

		private static bool IsPostgresException(DbUpdateException exception, string code, out string? constraintName)
		{
			if (exception.InnerException is PostgresException pgException && pgException.SqlState == code)
			{
				constraintName = pgException.ConstraintName;
				return true;
			}

			constraintName = null;
			return false;
		}
	}
}
