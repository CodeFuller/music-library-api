using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Genre
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();
	}
}
