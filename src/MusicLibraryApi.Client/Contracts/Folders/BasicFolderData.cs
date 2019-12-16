using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public abstract class BasicFolderData
	{
		[DataMember(Name = "name")]
		public string? Name { get; set; }
	}
}
