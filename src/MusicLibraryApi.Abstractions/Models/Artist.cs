using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Artist
	{
		public int Id { get; private set; }

		public string Name { get; set; }

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();

		public Artist(string name)
		{
			Name = name;
		}

		public Artist(int id, string name)
			: this(name)
		{
			Id = id;
		}
	}
}
