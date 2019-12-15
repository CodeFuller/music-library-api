using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Artist
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();
	}
}
