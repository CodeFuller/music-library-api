using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public abstract class BasicFolderData
	{
		[DataMember(Name = "name")]
		public string? Name { get; }

		[DataMember(Name = "parentFolderId")]
		public int? ParentFolderId { get; }

		protected BasicFolderData(string? name, int? parentFolderId)
		{
			Name = name;
			ParentFolderId = parentFolderId;
		}
	}
}
