namespace MusicLibraryApi.Client
{
	public static class GenreFields
	{
		public static QueryField Id { get; } = new QueryField("id");

		public static QueryField Name { get; } = new QueryField("name");

		public static QueryFieldSet All { get; } = Id + Name;
	}
}
