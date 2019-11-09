using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; }

		public string Name { get; }

		public Folder? ParentFolder { get; }

		public IReadOnlyCollection<Disc> Discs { get; } = new List<Disc>();

		public Folder(int id, string name, Folder? parentFolder)
		{
			Id = id;
			Name = name;
			ParentFolder = parentFolder;
		}
	}
}
