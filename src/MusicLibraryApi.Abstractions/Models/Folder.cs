using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public int? ParentFolderId { get; set; }

		public Folder? ParentFolder { get; set; }

		public IReadOnlyCollection<Folder> Subfolders { get; } = new List<Folder>();

		public IReadOnlyCollection<Disc> Discs { get; } = new List<Disc>();
	}
}
