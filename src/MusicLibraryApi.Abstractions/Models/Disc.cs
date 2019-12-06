using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Disc
	{
		public int Id { get; }

		public int? Year { get; }

		public string Title { get; }

		public string? TreeTitle { get; set; }

		public string AlbumTitle { get; }

		public string? AlbumId { get; }

		public int? AlbumOrder { get; }

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();

		public Folder Folder { get; private set; } = null!;

		public DateTimeOffset? DeleteDate { get; }

		public string? DeleteComment { get; }

		public bool IsDeleted => DeleteDate != null;

		public Disc(int? year, string title, string? treeTitle, string albumTitle, string? albumId, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
		{
			Year = year;
			Title = title;
			TreeTitle = treeTitle;
			AlbumTitle = albumTitle;
			AlbumId = albumId;
			AlbumOrder = albumOrder;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}

		public Disc(int id, int? year, string title, string? treeTitle, string albumTitle, string? albumId, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
			: this(year, title, treeTitle, albumTitle, albumId, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
