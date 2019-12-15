using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Playback
	{
		public int Id { get; set; }

		public int SongId { get; set; }

		public Song Song { get; set; } = null!;

		public DateTimeOffset PlaybackTime { get; set; }
	}
}
