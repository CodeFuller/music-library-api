using System;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Playback
	{
		public int Id { get; }

		public Song Song { get; }

		public DateTimeOffset PlaybackTime { get; }

		public Playback(int id, Song song, DateTimeOffset playbackTime)
		{
			Id = id;
			Song = song;
			PlaybackTime = playbackTime;
		}
	}
}
