using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public class InputFolderData : BasicFolderData
	{
		public InputFolderData(string name, int? parentFolderId)
			: base(name, parentFolderId)
		{
		}
	}
}
