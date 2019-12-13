using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Playback
	{
		public int Id { get; private set; }

		public int SongId { get; set; }

		public Song Song { get; private set; } = null!;

		public DateTimeOffset PlaybackTime { get; set; }

		public Playback(int songId, DateTimeOffset playbackTime)
		{
			SongId = songId;
			PlaybackTime = playbackTime;
		}

		public Playback(int id, int songId, DateTimeOffset playbackTime)
			: this(songId, playbackTime)
		{
			Id = id;
		}
	}
}
