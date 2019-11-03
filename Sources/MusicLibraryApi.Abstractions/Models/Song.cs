using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Song
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public short? TrackNumber { get; set; }

		public TimeSpan? Duration { get; set; }

		public Genre Genre { get; set; }

		public Rating? Rating { get; set; }

		public int? BitRate { get; set; }

		public int FileSize { get; set; }

		public int Checksum { get; set; }

		public Disc Disc { get; set; }

		public Artist Artist { get; set; }

		public DateTimeOffset? LastPlaybackTime { get; set; }

		public int PlaybacksCount { get; set; }

		public IReadOnlyCollection<Playback> Playbacks { get; } = new List<Playback>();
	}
}
