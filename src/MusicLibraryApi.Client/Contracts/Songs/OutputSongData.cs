using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class OutputSongData : BasicSongData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputSongData(int? id = null, string? title = null, string? treeTitle = null, short? trackNumber = null, TimeSpan? duration = null, Rating? rating = null,
			int? bitRate = null, DateTimeOffset? lastPlaybackTime = null, int? playbacksCount = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(title, treeTitle, trackNumber, duration, rating, bitRate, lastPlaybackTime, playbacksCount, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
