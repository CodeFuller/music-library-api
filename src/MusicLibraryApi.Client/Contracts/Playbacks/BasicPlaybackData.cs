using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Playbacks
{
	[DataContract]
	public abstract class BasicPlaybackData
	{
		[DataMember(Name = "playbackTime")]
		public DateTimeOffset? PlaybackTime { get; }

		protected BasicPlaybackData(DateTimeOffset? playbackTime)
		{
			PlaybackTime = playbackTime;
		}
	}
}
