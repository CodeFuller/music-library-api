using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Playback
	{
		public int Id { get; }

		public int SongId { get; }

		public DateTimeOffset PlaybackTime { get; }

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
