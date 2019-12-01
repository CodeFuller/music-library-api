using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public class InputFolderData : BasicFolderData
	{
		[DataMember(Name = "parentFolderId")]
		public int? ParentFolderId { get; }

		public InputFolderData(string name, int? parentFolderId)
			: base(name)
		{
			ParentFolderId = parentFolderId;
		}
	}
}
