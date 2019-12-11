using System;
using System.Collections.Generic;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class SongEntity
	{
		public int Id { get; private set; }

		public string Title { get; private set; }

		public string TreeTitle { get; private set; }

		public short? TrackNumber { get; private set; }

		public TimeSpan Duration { get; private set; }

		public int DiscId { get; private set; }

		public DiscEntity Disc { get; set; } = null!;

		public int? ArtistId { get; private set; }

		public ArtistEntity? Artist { get; set; }

		public int? GenreId { get; private set; }

		public GenreEntity? Genre { get; set; }

		public Rating? Rating { get; private set; }

		public int? BitRate { get; private set; }

		public DateTimeOffset? LastPlaybackTime { get; private set; }

		public int PlaybacksCount { get; private set; }

		public IReadOnlyCollection<PlaybackEntity> Playbacks { get; } = new List<PlaybackEntity>();

		public DateTimeOffset? DeleteDate { get; private set; }

		public string? DeleteComment { get; private set; }

		public SongEntity(int id, string title, string treeTitle, short? trackNumber, TimeSpan duration, int discId, int? artistId, int? genreId,
			Rating? rating, int? bitRate, DateTimeOffset? lastPlaybackTime, int playbacksCount, DateTimeOffset? deleteDate = null, string? deleteComment = null)
		{
			Id = id;
			Title = title;
			TreeTitle = treeTitle;
			TrackNumber = trackNumber;
			Duration = duration;
			DiscId = discId;
			ArtistId = artistId;
			GenreId = genreId;
			Rating = rating;
			BitRate = bitRate;
			LastPlaybackTime = lastPlaybackTime;
			PlaybacksCount = playbacksCount;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}
	}
}
