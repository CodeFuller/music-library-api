namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; }

		public string Name { get; }

		public Folder(string name)
		{
			Name = name;
		}

		public Folder(int id, string name)
			: this(name)
		{
			Id = id;
		}
	}
}
