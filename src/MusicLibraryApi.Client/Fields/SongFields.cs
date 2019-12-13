using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Fields
{
	public static class SongFields
	{
		public static QueryField<OutputSongData> Id { get; } = new QueryField<OutputSongData>("id");

		public static QueryField<OutputSongData> Title { get; } = new QueryField<OutputSongData>("title");

		public static QueryField<OutputSongData> TreeTitle { get; } = new QueryField<OutputSongData>("treeTitle");

		public static QueryField<OutputSongData> TrackNumber { get; } = new QueryField<OutputSongData>("trackNumber");

		public static QueryField<OutputSongData> Duration { get; } = new QueryField<OutputSongData>("duration");

		public static QueryField<OutputSongData> Rating { get; } = new QueryField<OutputSongData>("rating");

		public static QueryField<OutputSongData> BitRate { get; } = new QueryField<OutputSongData>("bitRate");

		public static QueryField<OutputSongData> LastPlaybackTime { get; } = new QueryField<OutputSongData>("lastPlaybackTime");

		public static QueryField<OutputSongData> PlaybacksCount { get; } = new QueryField<OutputSongData>("playbacksCount");

		public static ComplexQueryField<OutputSongData, OutputDiscData> Disc(QueryFieldSet<OutputDiscData> discFields)
		{
			return new ComplexQueryField<OutputSongData, OutputDiscData>("disc", discFields);
		}

		public static ComplexQueryField<OutputSongData, OutputArtistData> Artist(QueryFieldSet<OutputArtistData> artistFields)
		{
			return new ComplexQueryField<OutputSongData, OutputArtistData>("artist", artistFields);
		}

		public static ComplexQueryField<OutputSongData, OutputGenreData> Genre(QueryFieldSet<OutputGenreData> genreFields)
		{
			return new ComplexQueryField<OutputSongData, OutputGenreData>("genre", genreFields);
		}

		public static ComplexQueryField<OutputSongData, OutputPlaybackData> Playbacks(QueryFieldSet<OutputPlaybackData> playbackFields)
		{
			return new ComplexQueryField<OutputSongData, OutputPlaybackData>("playbacks", playbackFields);
		}

		public static QueryField<OutputSongData> DeleteDate { get; } = new QueryField<OutputSongData>("deleteDate");

		public static QueryField<OutputSongData> DeleteComment { get; } = new QueryField<OutputSongData>("deleteComment");

		public static QueryFieldSet<OutputSongData> All { get; } = Id + Title + TreeTitle + TrackNumber + Duration + Rating + BitRate + LastPlaybackTime + PlaybacksCount + DeleteDate + DeleteComment;
	}
}
