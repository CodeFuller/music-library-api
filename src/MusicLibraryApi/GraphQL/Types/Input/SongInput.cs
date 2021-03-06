﻿using System;
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

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public Song ToModel()
		{
			if (DiscId == null)
			{
				throw new InvalidOperationException("Song disc is not set");
			}

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

			return new Song
			{
				Title = Title,
				TreeTitle = TreeTitle,
				TrackNumber = TrackNumber,
				Duration = Duration.Value,
				DiscId = DiscId.Value,
				ArtistId = ArtistId,
				GenreId = GenreId,
				Rating = Rating,
				BitRate = BitRate,
				DeleteDate = DeleteDate,
				DeleteComment = DeleteComment,
			};
		}
	}
}
