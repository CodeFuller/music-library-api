using System;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class PlaybackEntity
	{
		public int Id { get; private set; }

		public SongEntity Song { get; private set; } = null!;

		public DateTimeOffset PlaybackTime { get; private set; }

		public PlaybackEntity(int id, DateTimeOffset playbackTime)
		{
			Id = id;
			PlaybackTime = playbackTime;
		}
	}
}
