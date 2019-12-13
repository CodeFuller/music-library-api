using System;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Playbacks
{
	[DataContract]
	public class OutputPlaybackData : BasicPlaybackData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "song")]
		public OutputSongData? Song { get; }

		public OutputPlaybackData(int? id = null, DateTimeOffset? playbackTime = null, OutputSongData? song = null)
			: base(playbackTime)
		{
			Id = id;
			Song = song;
		}
	}
}
