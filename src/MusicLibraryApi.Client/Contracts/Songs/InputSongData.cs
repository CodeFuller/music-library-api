using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class InputSongData : BasicSongData
	{
		[DataMember(Name = "discId")]
		public int DiscId { get; }

		[DataMember(Name = "artistId")]
		public int? ArtistId { get; }

		[DataMember(Name = "genreId")]
		public int? GenreId { get; }

		public InputSongData(int discId, int? artistId, int? genreId, string title, string treeTitle, short? trackNumber, TimeSpan duration, Rating? rating,
			int? bitRate, DateTimeOffset? lastPlaybackTime = null, int playbacksCount = 0, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(title, treeTitle, trackNumber, duration, rating, bitRate, lastPlaybackTime, playbacksCount, deleteDate, deleteComment)
		{
			DiscId = discId;
			ArtistId = artistId;
			GenreId = genreId;
		}
	}
}
