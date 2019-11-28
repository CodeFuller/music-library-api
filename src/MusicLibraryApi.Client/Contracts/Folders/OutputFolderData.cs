using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public class OutputFolderData : BasicFolderData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputFolderData(int? id, string name, int? parentFolderId)
			: base(name, parentFolderId)
		{
			Id = id;
		}
	}
}
