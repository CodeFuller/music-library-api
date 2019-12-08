using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class OutputSongData : BasicSongData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		public OutputSongData(int? id, string? title, string? treeTitle, short? trackNumber, TimeSpan? duration, Rating? rating,
			int? bitRate, DateTimeOffset? lastPlaybackTime, int? playbacksCount, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(title, treeTitle, trackNumber, duration, rating, bitRate, lastPlaybackTime, playbacksCount, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
