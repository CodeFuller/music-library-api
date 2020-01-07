using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Song
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;

		public string TreeTitle { get; set; } = null!;

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

		public long? Size { get; set; }

		public uint? Checksum { get; set; }

		public DateTimeOffset? LastPlaybackTime { get; set; }

		public int PlaybacksCount { get; set; }

		public IReadOnlyCollection<Playback> Playbacks { get; } = new List<Playback>();

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public bool IsDeleted => DeleteDate != null;
	}
}
