namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; }

		public string Name { get; }

		public int? ParentFolderId { get; }

		public Folder(string name, int? parentFolderId)
		{
			Name = name;
			ParentFolderId = parentFolderId;
		}

		public Folder(int id, string name, int? parentFolderId)
			: this(name, parentFolderId)
		{
			Id = id;
		}
	}
}
