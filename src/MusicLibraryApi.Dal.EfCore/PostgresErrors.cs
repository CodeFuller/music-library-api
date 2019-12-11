namespace MusicLibraryApi.Dal.EfCore
{
	internal static class PostgresErrors
	{
		public static string ForeignKeyViolation => "23503";

		public static string UniqueViolation => "23505";
	}
}
