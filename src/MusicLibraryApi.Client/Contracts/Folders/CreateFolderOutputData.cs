using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public class CreateFolderOutputData
	{
		[DataMember(Name = "newFolderId")]
		public int? NewFolderId { get; }

		public CreateFolderOutputData(int? newFolderId)
		{
			NewFolderId = newFolderId;
		}
	}
}
