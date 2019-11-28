using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class FolderEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public FolderEntity? ParentFolder { get; set; }

		public IReadOnlyCollection<FolderEntity> ChildFolders { get; } = new List<FolderEntity>();

		public FolderEntity(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
