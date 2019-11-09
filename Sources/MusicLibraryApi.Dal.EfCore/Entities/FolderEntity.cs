using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class FolderEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public FolderEntity? ParentFolder { get; private set; }

		public IReadOnlyCollection<FolderEntity> ChildFolders { get; } = new List<FolderEntity>();

		public IReadOnlyCollection<DiscEntity> Discs { get; } = new List<DiscEntity>();

		public FolderEntity(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
