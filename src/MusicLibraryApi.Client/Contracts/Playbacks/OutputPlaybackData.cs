using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Playbacks
{
	[DataContract]
	public class OutputPlaybackData : BasicPlaybackData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		[DataMember(Name = "song")]
		public OutputSongData? Song { get; set; }
	}
}
