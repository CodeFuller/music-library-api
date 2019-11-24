using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class GenreFields
	{
		public static QueryField<GenreQuery> Id { get; } = new QueryField<GenreQuery>("id");

		public static QueryField<GenreQuery> Name { get; } = new QueryField<GenreQuery>("name");

		public static QueryFieldSet<GenreQuery> All { get; } = Id + Name;
	}
}
