namespace MusicLibraryApi.Abstractions.Models
{
	public class Artist
	{
		public int Id { get; }

		public string Name { get; }

		public Artist(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
