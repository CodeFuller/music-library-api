using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class DiscFields
	{
		public static QueryField<DiscQuery> Id { get; } = new QueryField<DiscQuery>("id");

		public static QueryField<DiscQuery> Year { get; } = new QueryField<DiscQuery>("year");

		public static QueryField<DiscQuery> Title { get; } = new QueryField<DiscQuery>("title");

		public static QueryField<DiscQuery> AlbumTitle { get; } = new QueryField<DiscQuery>("albumTitle");

		public static QueryField<DiscQuery> AlbumId { get; } = new QueryField<DiscQuery>("albumId");

		public static QueryField<DiscQuery> AlbumOrder { get; } = new QueryField<DiscQuery>("albumOrder");

		public static QueryField<DiscQuery> DeleteDate { get; } = new QueryField<DiscQuery>("deleteDate");

		public static QueryField<DiscQuery> DeleteComment { get; } = new QueryField<DiscQuery>("deleteComment");

		public static QueryFieldSet<DiscQuery> All { get; } = Id + Year + Title + AlbumTitle + AlbumId + AlbumOrder + DeleteDate + DeleteComment;
	}
}
