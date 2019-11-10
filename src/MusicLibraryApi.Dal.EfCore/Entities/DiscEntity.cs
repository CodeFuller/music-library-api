using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class DiscEntity
	{
		public int Id { get; private set; }

		public int? Year { get; private set; }

		public string Title { get; private set; }

		public string? AlbumTitle { get; private set; }

		public int? AlbumOrder { get; private set; }

		// CF TEMP: Make all other properties also settable?
		public FolderEntity Folder { get; set; } = null!;

		public DateTimeOffset? DeleteDate { get; private set; }

		public string? DeleteComment { get; private set; }

		public IReadOnlyCollection<SongEntity> Songs { get; } = new List<SongEntity>();

		public DiscEntity(int id, int? year, string title, string? albumTitle = null, int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
		{
			Id = id;
			Year = year;
			Title = title;
			AlbumTitle = albumTitle;
			AlbumOrder = albumOrder;

			// We convert date to universal time mostly because of integration tests.
			// PostgreSQL does not store timezone in 'timestamp with timezone' column.
			// When the value is read from the database, the local timezone is set.
			// This makes difficult baseline-based testing.
			DeleteDate = deleteDate?.ToUniversalTime();
			DeleteComment = deleteComment;
		}
	}
}
