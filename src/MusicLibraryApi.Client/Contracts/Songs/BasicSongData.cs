using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public abstract class BasicSongData
	{
		[DataMember(Name = "title")]
		public string? Title { get; }

		[DataMember(Name = "treeTitle")]
		public string? TreeTitle { get; }

		[DataMember(Name = "trackNumber")]
		public short? TrackNumber { get; }

		[DataMember(Name = "duration")]
		public TimeSpan? Duration { get; }

		[DataMember(Name = "rating")]
		public Rating? Rating { get; }

		[DataMember(Name = "bitRate")]
		public int? BitRate { get; }

		[DataMember(Name = "lastPlaybackTime")]
		public DateTimeOffset? LastPlaybackTime { get; }

		[DataMember(Name = "playbacksCount")]
		public int? PlaybacksCount { get; }

		[DataMember(Name = "deleteDate")]
		public DateTimeOffset? DeleteDate { get; }

		[DataMember(Name = "deleteComment")]
		public string? DeleteComment { get; }

		protected BasicSongData(string? title, string? treeTitle, short? trackNumber, TimeSpan? duration, Rating? rating,
			int? bitRate, DateTimeOffset? lastPlaybackTime, int? playbacksCount, DateTimeOffset? deleteDate, string? deleteComment)
		{
			Title = title;
			TreeTitle = treeTitle;
			TrackNumber = trackNumber;
			Duration = duration;
			Rating = rating;
			BitRate = bitRate;
			LastPlaybackTime = lastPlaybackTime;
			PlaybacksCount = playbacksCount;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}
	}
}
