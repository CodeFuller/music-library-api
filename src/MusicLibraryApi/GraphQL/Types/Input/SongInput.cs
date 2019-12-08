using System;
using System.Collections.Generic;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class SongInput
	{
		public int? DiscId { get; set; }

		public int? ArtistId { get; set; }

		public int? GenreId { get; set; }

		public string? Title { get; set; }

		public string? TreeTitle { get; set; }

		public short? TrackNumber { get; set; }

		public TimeSpan? Duration { get; set; }

		public Rating? Rating { get; set; }

		public int? BitRate { get; set; }

		public DateTimeOffset? LastPlaybackTime { get; set; }

		public int? PlaybacksCount { get; set; }

		public IReadOnlyCollection<Playback>? Playbacks { get; set; }

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public Song ToModel()
		{
			if (Title == null)
			{
				throw new InvalidOperationException("Song title is not set");
			}

			if (TreeTitle == null)
			{
				throw new InvalidOperationException("Song tree title is not set");
			}

			if (Duration == null)
			{
				throw new InvalidOperationException("Song duration is not set");
			}

			var playbacks = Playbacks ?? Array.Empty<Playback>();

			return new Song(Title, TreeTitle, TrackNumber, Duration.Value, Rating, BitRate, LastPlaybackTime, PlaybacksCount ?? 0, playbacks, DeleteDate, DeleteComment);
		}
	}
}
