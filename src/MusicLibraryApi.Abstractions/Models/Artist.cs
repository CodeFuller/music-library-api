namespace MusicLibraryApi.Abstractions.Models
{
	public class Artist
	{
		public int Id { get; }

		public string Name { get; }

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
