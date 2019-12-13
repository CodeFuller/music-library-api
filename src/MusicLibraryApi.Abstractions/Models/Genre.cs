using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Genre
	{
		public int Id { get; private set; }

		public string Name { get; set; }

		public IReadOnlyCollection<Song> Songs { get; } = new List<Song>();

		public Genre(string name)
		{
			Name = name;
		}

		public Genre(int id, string name)
			: this(name)
		{
			Id = id;
		}
	}
}
