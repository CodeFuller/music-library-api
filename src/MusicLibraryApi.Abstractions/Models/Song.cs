using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Song
	{
		public int Id { get; }

		public string Title { get; }

		public string TreeTitle { get; }

		public short? TrackNumber { get; }

		public TimeSpan Duration { get; }

		public Disc Disc { get; private set; } = null!;

		public Artist? Artist { get; private set; }

		public Genre? Genre { get; private set; }

		public Rating? Rating { get; }

		public int? BitRate { get; }

		public DateTimeOffset? LastPlaybackTime { get; }

		public int PlaybacksCount { get; }

		public DateTimeOffset? DeleteDate { get; }

		public string? DeleteComment { get; }

		public bool IsDeleted => DeleteDate != null;

		public Song(string title, string treeTitle, short? trackNumber, TimeSpan duration, Rating? rating, int? bitRate,
			DateTimeOffset? lastPlaybackTime, int playbacksCount, DateTimeOffset? deleteDate, string? deleteComment)
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

		public Song(int id, string title, string treeTitle, short? trackNumber, TimeSpan duration, Rating? rating, int? bitRate,
			DateTimeOffset? lastPlaybackTime, int playbacksCount, DateTimeOffset? deleteDate, string? deleteComment)
			: this(title, treeTitle, trackNumber, duration, rating, bitRate, lastPlaybackTime, playbacksCount, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
