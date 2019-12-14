using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts
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

		public OutputStatisticsData(int? artistsNumber = null, int? discArtistsNumber = null, int? discsNumber = null, int? songsNumber = null,
			TimeSpan? songsDuration = null, TimeSpan? playbacksDuration = null, int? playbacksNumber = null, int? unheardSongsNumber = null)
		{
			ArtistsNumber = artistsNumber;
			DiscArtistsNumber = discArtistsNumber;
			DiscsNumber = discsNumber;
			SongsNumber = songsNumber;
			SongsDuration = songsDuration;
			PlaybacksDuration = playbacksDuration;
			PlaybacksNumber = playbacksNumber;
			UnheardSongsNumber = unheardSongsNumber;
		}
	}
}
