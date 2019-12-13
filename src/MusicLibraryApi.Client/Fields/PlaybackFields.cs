using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Fields
{
	public static class PlaybackFields
	{
		public static QueryField<OutputPlaybackData> Id { get; } = new QueryField<OutputPlaybackData>("id");

		public static QueryField<OutputPlaybackData> PlaybackTime { get; } = new QueryField<OutputPlaybackData>("playbackTime");

		public static ComplexQueryField<OutputPlaybackData, OutputSongData> Song(QueryFieldSet<OutputSongData> songFields)
		{
			return new ComplexQueryField<OutputPlaybackData, OutputSongData>("song", songFields);
		}

		public static QueryFieldSet<OutputPlaybackData> All { get; } = Id + PlaybackTime;
	}
}
