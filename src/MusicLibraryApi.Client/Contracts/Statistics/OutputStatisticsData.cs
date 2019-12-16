using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Statistics
{
	[DataContract]
	public class OutputStatisticsData
	{
		[DataMember(Name = "artistsNumber")]
		public int? ArtistsNumber { get; set; }

		[DataMember(Name = "discArtistsNumber")]
		public int? DiscArtistsNumber { get; set; }

		[DataMember(Name = "discsNumber")]
		public int? DiscsNumber { get; set; }

		[DataMember(Name = "songsNumber")]
		public int? SongsNumber { get; set; }

		[DataMember(Name = "songsDuration")]
		public TimeSpan? SongsDuration { get; set; }

		[DataMember(Name = "playbacksDuration")]
		public TimeSpan? PlaybacksDuration { get; set; }

		[DataMember(Name = "playbacksNumber")]
		public int? PlaybacksNumber { get; set; }

		[DataMember(Name = "unheardSongsNumber")]
		public int? UnheardSongsNumber { get; set; }

		[DataMember(Name = "songsRatings")]
		public IReadOnlyCollection<RatingSongsData>? SongsRatings { get; set; }
	}
}
