using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Disc
	{
		public int Id { get; }

		public int? Year { get; }

		public string Title { get; }

		public string? AlbumTitle { get; }

		public int? AlbumOrder { get; }

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();

		public Folder Folder { get; private set; } = null!;

		public DateTimeOffset? DeleteDate { get; }

		public string? DeleteComment { get; }

		public bool IsDeleted => DeleteDate != null;

		public Disc(int? year, string title, string? albumTitle, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
		{
			Year = year;
			Title = title;
			AlbumTitle = albumTitle;
			AlbumOrder = albumOrder;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}

		public Disc(int id, int? year, string title, string? albumTitle, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
			: this(year, title, albumTitle, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
