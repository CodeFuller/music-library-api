using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Playbacks
{
	[DataContract]
	public class InputPlaybackData : BasicPlaybackData
	{
		[DataMember(Name = "songId")]
		public int? SongId { get; }

		public InputPlaybackData(int songId, DateTimeOffset? playbackTime)
			: base(playbackTime)
		{
			SongId = songId;
		}
	}
}
