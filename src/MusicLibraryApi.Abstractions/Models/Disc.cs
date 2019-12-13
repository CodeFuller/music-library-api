using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Disc
	{
		public int Id { get; private set; }

		public int? Year { get; set; }

		public string Title { get; set; }

		public string TreeTitle { get; set; }

		public string AlbumTitle { get; set; }

		public string? AlbumId { get; set; }

		public int? AlbumOrder { get; set; }

		public int FolderId { get; set; }

		public Folder Folder { get; set; } = null!;

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public bool IsDeleted => DeleteDate != null;

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();

		public Disc(int? year, string title, string treeTitle, string albumTitle, int folderId, string? albumId = null,
			int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
		{
			Year = year;
			Title = title;
			TreeTitle = treeTitle;
			AlbumTitle = albumTitle;
			AlbumId = albumId;
			AlbumOrder = albumOrder;
			FolderId = folderId;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}

		public Disc(int id, int? year, string title, string treeTitle, string albumTitle, int folderId,
			string? albumId = null, int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: this(year, title, treeTitle, albumTitle, folderId, albumId, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
