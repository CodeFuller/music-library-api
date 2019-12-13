using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Song
	{
		public int Id { get; private set; }

		public string Title { get; set; }

		public string TreeTitle { get; set; }

		public short? TrackNumber { get; set; }

		public TimeSpan Duration { get; set; }

		public int DiscId { get; set; }

		public Disc Disc { get; set; } = null!;

		public int? ArtistId { get; set; }

		public Artist? Artist { get; set; }

		public int? GenreId { get; set; }

		public Genre? Genre { get; set; }

		public Rating? Rating { get; set; }

		public int? BitRate { get; set; }

		public DateTimeOffset? LastPlaybackTime { get; set; }

		public int PlaybacksCount { get; set; }

		public IReadOnlyCollection<Playback> Playbacks { get; } = new List<Playback>();

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public bool IsDeleted => DeleteDate != null;

		public Song(string title, string treeTitle, short? trackNumber, TimeSpan duration, int discId, int? artistId, int? genreId,
			Rating? rating, int? bitRate, DateTimeOffset? lastPlaybackTime, int playbacksCount, DateTimeOffset? deleteDate = null, string? deleteComment = null)
		{
			Title = title;
			TreeTitle = treeTitle;
			TrackNumber = trackNumber;
			Duration = duration;
			DiscId = discId;
			ArtistId = artistId;
			GenreId = genreId;
			Rating = rating;
			BitRate = bitRate;
			LastPlaybackTime = lastPlaybackTime;
			PlaybacksCount = playbacksCount;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}

		public Song(int id, string title, string treeTitle, short? trackNumber, TimeSpan duration, int discId, int? artistId, int? genreId,
			Rating? rating, int? bitRate, DateTimeOffset? lastPlaybackTime, int playbacksCount, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: this(title, treeTitle, trackNumber, duration, discId, artistId, genreId, rating, bitRate, lastPlaybackTime, playbacksCount, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
