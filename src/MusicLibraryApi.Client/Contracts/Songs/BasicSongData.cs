using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public abstract class BasicSongData
	{
		[DataMember(Name = "title")]
		public string? Title { get; set; }

		[DataMember(Name = "treeTitle")]
		public string? TreeTitle { get; set; }

		[DataMember(Name = "trackNumber")]
		public short? TrackNumber { get; set; }

		[DataMember(Name = "duration")]
		public TimeSpan? Duration { get; set; }

		[DataMember(Name = "rating")]
		public Rating? Rating { get; set; }

		[DataMember(Name = "bitRate")]
		public int? BitRate { get; set; }

		[DataMember(Name = "lastPlaybackTime")]
		public DateTimeOffset? LastPlaybackTime { get; set; }

		[DataMember(Name = "playbacksCount")]
		public int? PlaybacksCount { get; set; }

		[DataMember(Name = "deleteDate")]
		public DateTimeOffset? DeleteDate { get; set; }

		[DataMember(Name = "deleteComment")]
		public string? DeleteComment { get; set; }
	}
}
