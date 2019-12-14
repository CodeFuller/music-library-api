using MusicLibraryApi.Client.Contracts;

namespace MusicLibraryApi.Client.Fields
{
	public static class StatisticsFields
	{
		public static QueryField<OutputStatisticsData> ArtistsNumber { get; } = new QueryField<OutputStatisticsData>("artistsNumber");

		public static QueryField<OutputStatisticsData> DiscArtistsNumber { get; } = new QueryField<OutputStatisticsData>("discArtistsNumber");

		public static QueryField<OutputStatisticsData> DiscsNumber { get; } = new QueryField<OutputStatisticsData>("discsNumber");

		public static QueryField<OutputStatisticsData> SongsNumber { get; } = new QueryField<OutputStatisticsData>("songsNumber");

		public static QueryField<OutputStatisticsData> SongsDuration { get; } = new QueryField<OutputStatisticsData>("songsDuration");

		public static QueryField<OutputStatisticsData> PlaybacksDuration { get; } = new QueryField<OutputStatisticsData>("playbacksDuration");

		public static QueryField<OutputStatisticsData> PlaybacksNumber { get; } = new QueryField<OutputStatisticsData>("playbacksNumber");

		public static QueryField<OutputStatisticsData> UnheardSongsNumber { get; } = new QueryField<OutputStatisticsData>("unheardSongsNumber");

		public static QueryFieldSet<OutputStatisticsData> All { get; } = ArtistsNumber + DiscArtistsNumber + DiscsNumber + SongsNumber +
		                                                                 SongsDuration + PlaybacksDuration + PlaybacksNumber + UnheardSongsNumber;
	}
}
