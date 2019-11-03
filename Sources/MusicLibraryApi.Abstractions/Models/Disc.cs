using System;
using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Disc
	{
		public int Id { get; set; }

		public int? Year { get; set; }

		public string Title { get; set; }

		public string AlbumTitle { get; set; }

		public int? AlbumOrder { get; set; }

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();

		public Folder Folder { get; set; }

		public DateTimeOffset? DeleteDate { get; set; }

		public string DeleteComment { get; set; }
	}
}
