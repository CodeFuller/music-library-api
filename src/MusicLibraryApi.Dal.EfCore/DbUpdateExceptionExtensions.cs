using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MusicLibraryApi.Dal.EfCore
{
	internal static class DbUpdateExceptionExtensions
	{
		public static bool IsUniqueViolationException(this DbUpdateException exception)
		{
			return IsPostgresException(exception, PostgresErrors.UniqueViolation);
		}

		public static bool IsForeignKeyViolationException(this DbUpdateException exception)
		{
			return IsPostgresException(exception, PostgresErrors.ForeignKeyViolation);
		}

		public static bool IsForeignKeyViolationException(this DbUpdateException exception, string constraintName)
		{
			return IsPostgresException(exception, PostgresErrors.ForeignKeyViolation, constraintName);
		}

		private static bool IsPostgresException(DbUpdateException exception, string code, string? constraintName = null)
		{
			return exception.InnerException is PostgresException pgException && pgException.SqlState == code &&
			       (String.IsNullOrEmpty(constraintName) || String.Equals(pgException.ConstraintName, constraintName, StringComparison.Ordinal));
		}
	}
}
