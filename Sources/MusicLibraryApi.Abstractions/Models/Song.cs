using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Song
	{
		public string Id { get; set; }

		public string Title { get; set; }

		public short? TrackNumber { get; set; }

		public TimeSpan? Duration { get; set; }

		public Disc Disc { get; set; }
	}
}
