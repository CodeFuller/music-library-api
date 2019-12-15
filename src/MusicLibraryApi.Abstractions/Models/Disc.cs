using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Disc
	{
		public int Id { get; set; }

		public int? Year { get; set; }

		public string Title { get; set; } = null!;

		public string TreeTitle { get; set; } = null!;

		public string AlbumTitle { get; set; } = null!;

		public string? AlbumId { get; set; }

		public int? AlbumOrder { get; set; }

		public int FolderId { get; set; }

		public Folder Folder { get; set; } = null!;

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public bool IsDeleted => DeleteDate != null;

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();
	}
}
