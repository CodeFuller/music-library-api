using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public Folder ParentFolder { get; set; }

		public IReadOnlyCollection<Disc> Discs { get; } = new List<Disc>();
	}
}
