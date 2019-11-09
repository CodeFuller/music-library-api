using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class DiscEntity
	{
		public int Id { get; set; }

		public int? Year { get; set; }

		public string Title { get; set; }

		public string AlbumTitle { get; set; }

		public int? AlbumOrder { get; set; }

		public FolderEntity Folder { get; set; }

		public DateTimeOffset? DeleteDate { get; set; }

		public string DeleteComment { get; set; }

		public IReadOnlyCollection<SongEntity> Songs { get; } = new List<SongEntity>();
	}
}
