using System;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class PlaybackInput
	{
		public int? SongId { get; set; }

		public DateTimeOffset? PlaybackTime { get; set; }

		public Playback ToModel()
		{
			if (SongId == null)
			{
				throw new InvalidOperationException("Playback song is not set");
			}

			if (PlaybackTime == null)
			{
				throw new InvalidOperationException("Playback time is not set");
			}

			return new Playback
			{
				SongId = SongId.Value,
				PlaybackTime = PlaybackTime.Value,
			};
		}
	}
}
