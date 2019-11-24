namespace MusicLibraryApi.Client.Fields
{
	public static class DiscFields
	{
		public static QueryField Id { get; } = new QueryField("id");

		public static QueryField Year { get; } = new QueryField("year");

		public static QueryField Title { get; } = new QueryField("title");

		public static QueryField AlbumTitle { get; } = new QueryField("albumTitle");

		public static QueryField AlbumOrder { get; } = new QueryField("albumOrder");

		public static QueryField DeleteDate { get; } = new QueryField("deleteDate");

		public static QueryField DeleteComment { get; } = new QueryField("deleteComment");

		public static QueryFieldSet All { get; } = Id + Year + Title + AlbumTitle + AlbumOrder + DeleteDate + DeleteComment;
	}
}
