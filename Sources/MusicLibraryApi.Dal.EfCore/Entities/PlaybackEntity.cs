using System;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class PlaybackEntity
	{
		public int Id { get; set; }

		public SongEntity Song { get; set; }

		public DateTimeOffset PlaybackTime { get; set; }
	}
}
