using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class FolderEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public int? ParentFolderId { get; private set; }

		public FolderEntity? ParentFolder { get; set; }

		public IReadOnlyCollection<FolderEntity> Subfolders { get; } = new List<FolderEntity>();

		public IReadOnlyCollection<DiscEntity> Discs { get; } = new List<DiscEntity>();

		public FolderEntity(int id, string name, int? parentFolderId)
		{
			Id = id;
			Name = name;
			ParentFolderId = parentFolderId;
		}
	}
}
