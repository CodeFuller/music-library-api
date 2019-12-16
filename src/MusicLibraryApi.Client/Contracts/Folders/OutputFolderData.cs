using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Discs;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public class OutputFolderData : BasicFolderData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		[DataMember(Name = "subfolders")]
		public IReadOnlyCollection<OutputFolderData>? Subfolders { get; set; }

		[DataMember(Name = "discs")]
		public IReadOnlyCollection<OutputDiscData>? Discs { get; set; }
	}
}
