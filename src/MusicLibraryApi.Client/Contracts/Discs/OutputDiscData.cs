using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class OutputDiscData : BasicDiscData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		[DataMember(Name = "folder")]
		public OutputFolderData? Folder { get; set; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; set; }
	}
}
