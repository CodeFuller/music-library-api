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

		public FolderEntity? Folder { get; set; }

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
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}
	}
}
