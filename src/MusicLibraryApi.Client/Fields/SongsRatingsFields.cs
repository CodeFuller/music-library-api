using MusicLibraryApi.Client.Contracts.Statistics;

namespace MusicLibraryApi.Client.Fields
{
	public static class SongsRatingsFields
	{
		public static QueryField<RatingSongsData> Rating { get; } = new QueryField<RatingSongsData>("rating");

		public static QueryField<RatingSongsData> SongsNumber { get; } = new QueryField<RatingSongsData>("songsNumber");

		public static QueryFieldSet<RatingSongsData> All { get; } = Rating + SongsNumber;
	}
}
