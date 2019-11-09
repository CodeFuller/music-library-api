using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class FolderEntity
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public FolderEntity ParentFolder { get; set; }

		public IReadOnlyCollection<DiscEntity> Discs { get; } = new List<DiscEntity>();
	}
}
