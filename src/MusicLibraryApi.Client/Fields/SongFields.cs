using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class SongFields
	{
		public static QueryField<SongQuery> Id { get; } = new QueryField<SongQuery>("id");

		public static QueryField<SongQuery> Title { get; } = new QueryField<SongQuery>("title");

		public static QueryField<SongQuery> TreeTitle { get; } = new QueryField<SongQuery>("treeTitle");

		public static QueryField<SongQuery> TrackNumber { get; } = new QueryField<SongQuery>("trackNumber");

		public static QueryField<SongQuery> Duration { get; } = new QueryField<SongQuery>("duration");

		public static QueryField<SongQuery> Rating { get; } = new QueryField<SongQuery>("rating");

		public static QueryField<SongQuery> BitRate { get; } = new QueryField<SongQuery>("bitRate");

		public static QueryField<SongQuery> LastPlaybackTime { get; } = new QueryField<SongQuery>("lastPlaybackTime");

		public static QueryField<SongQuery> PlaybacksCount { get; } = new QueryField<SongQuery>("playbacksCount");

		public static ComplexQueryField<SongQuery, DiscQuery> Disc(QueryFieldSet<DiscQuery> discFields)
		{
			return new ComplexQueryField<SongQuery, DiscQuery>("disc", discFields);
		}

		public static ComplexQueryField<SongQuery, ArtistQuery> Artist(QueryFieldSet<ArtistQuery> artistFields)
		{
			return new ComplexQueryField<SongQuery, ArtistQuery>("artist", artistFields);
		}

		public static ComplexQueryField<SongQuery, GenreQuery> Genre(QueryFieldSet<GenreQuery> genreFields)
		{
			return new ComplexQueryField<SongQuery, GenreQuery>("genre", genreFields);
		}

		public static QueryField<SongQuery> DeleteDate { get; } = new QueryField<SongQuery>("deleteDate");

		public static QueryField<SongQuery> DeleteComment { get; } = new QueryField<SongQuery>("deleteComment");

		public static QueryFieldSet<SongQuery> All { get; } = Id + Title + TreeTitle + TrackNumber + Duration + Rating + BitRate + LastPlaybackTime + PlaybacksCount + DeleteDate + DeleteComment;
	}
}
