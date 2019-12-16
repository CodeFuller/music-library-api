using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class CreateSongOutputData
	{
		[DataMember(Name = "newSongId")]
		public int? NewSongId { get; set; }
	}
}
