using System;
using System.Collections.Generic;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class SongEntity
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public short? TrackNumber { get; set; }

		public TimeSpan? Duration { get; set; }

		public ArtistEntity Artist { get; set; }

		public DiscEntity Disc { get; set; }

		public GenreEntity Genre { get; set; }

		public Rating? Rating { get; set; }

		public int? BitRate { get; set; }

		public int FileSize { get; set; }

		public int Checksum { get; set; }

		public DateTimeOffset? LastPlaybackTime { get; set; }

		public int PlaybacksCount { get; set; }

		public IReadOnlyCollection<PlaybackEntity> Playbacks { get; } = new List<PlaybackEntity>();
	}
}
