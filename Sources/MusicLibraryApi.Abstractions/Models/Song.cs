using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Song
	{
		public int Id { get; }

		public string Title { get; }

		public short? TrackNumber { get; }

		public TimeSpan? Duration { get; }

		public Artist? Artist { get; private set; }

		public Disc? Disc { get; private set; } = null!;

		public Genre? Genre { get; private set; }

		public Rating? Rating { get; }

		public int? BitRate { get; }

		public int FileSize { get; }

		public int Checksum { get; }

		public DateTimeOffset? LastPlaybackTime { get; }

		public int PlaybacksCount { get; }

		public IReadOnlyCollection<Playback> Playbacks { get; }

		public Song(int id, string title, short? trackNumber, TimeSpan? duration, Rating? rating, int? bitRate,
			int fileSize, int checksum, DateTimeOffset? lastPlaybackTime, int playbacksCount, IEnumerable<Playback> playbacks)
		{
			Id = id;
			Title = title;
			TrackNumber = trackNumber;
			Duration = duration;
			Rating = rating;
			BitRate = bitRate;
			FileSize = fileSize;
			Checksum = checksum;
			LastPlaybackTime = lastPlaybackTime;
			PlaybacksCount = playbacksCount;
			Playbacks = playbacks.ToList();
		}
	}
}
