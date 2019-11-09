namespace MusicLibraryApi.Abstractions.Models
{
	public class Genre
	{
		public int Id { get; }

		public string Name { get; }

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
