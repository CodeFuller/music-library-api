using System;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class PlaybackEntity
	{
		public int Id { get; private set; }

		public int SongId { get; }

		public SongEntity Song { get; private set; } = null!;

		public DateTimeOffset PlaybackTime { get; private set; }

		public PlaybackEntity(int id, int songId, DateTimeOffset playbackTime)
		{
			Id = id;
			SongId = songId;
			PlaybackTime = playbackTime;
		}
	}
}
