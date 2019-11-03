using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Playback
	{
		public int Id { get; set; }

		public Song Song { get; set; }

		public DateTimeOffset PlaybackTime { get; set; }
	}
}
