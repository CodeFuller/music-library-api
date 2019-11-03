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

		public ICollection<Song> Songs { get; } = new List<Song>();
	}
}
