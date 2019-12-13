using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; private set; }

		public string Name { get; set; }

		public int? ParentFolderId { get; set; }

		public Folder? ParentFolder { get; set; }

		public IReadOnlyCollection<Folder> Subfolders { get; } = new List<Folder>();

		public IReadOnlyCollection<Disc> Discs { get; } = new List<Disc>();

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
