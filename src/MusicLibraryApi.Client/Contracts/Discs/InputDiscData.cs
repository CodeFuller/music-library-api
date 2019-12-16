using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class InputDiscData : BasicDiscData
	{
		[DataMember(Name = "folderId")]
		public int FolderId { get; set; }
	}
}
