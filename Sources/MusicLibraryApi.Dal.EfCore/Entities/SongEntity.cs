using System;
using System.Collections.Generic;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class SongEntity
	{
		public int Id { get; private set; }

		public string Title { get; private set; }

		public short? TrackNumber { get; private set; }

		public TimeSpan? Duration { get; private set; }

		public ArtistEntity? Artist { get; private set; }

		public DiscEntity Disc { get; private set; } = null!;

		public GenreEntity? Genre { get; private set; }

		public Rating? Rating { get; private set; }

		public int? BitRate { get; private set; }

		public int FileSize { get; private set; }

		public int Checksum { get; private set; }

		public DateTimeOffset? LastPlaybackTime { get; private set; }

		public int PlaybacksCount { get; private set; }

		public IReadOnlyCollection<PlaybackEntity> Playbacks { get; } = new List<PlaybackEntity>();

		public SongEntity(int id, string title, short? trackNumber, TimeSpan? duration, Rating? rating, int? bitRate,
			int fileSize, int checksum, DateTimeOffset? lastPlaybackTime, int playbacksCount)
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
		}
	}
}
