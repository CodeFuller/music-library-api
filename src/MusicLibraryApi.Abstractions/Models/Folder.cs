namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; }

		public string Name { get; }

		public Folder? ParentFolder { get; }

		public Folder(string name, Folder? parentFolder)
		{
			Name = name;
			ParentFolder = parentFolder;
		}

		public Folder(int id, string name, Folder? parentFolder)
			: this(name, parentFolder)
		{
			Id = id;
		}
	}
}
