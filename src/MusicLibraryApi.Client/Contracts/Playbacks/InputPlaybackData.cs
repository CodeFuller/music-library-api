using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Playbacks
{
	[DataContract]
	public class InputPlaybackData : BasicPlaybackData
	{
		[DataMember(Name = "songId")]
		public int? SongId { get; set; }
	}
}
