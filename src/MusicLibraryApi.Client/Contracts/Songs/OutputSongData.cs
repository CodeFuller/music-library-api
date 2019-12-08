using System;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class OutputSongData : BasicSongData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "disc")]
		public OutputDiscData? Disc { get; }

		[DataMember(Name = "artist")]
		public OutputArtistData? Artist { get; }

		[DataMember(Name = "genre")]
		public OutputGenreData? Genre { get; }

		public OutputSongData(int? id = null, string? title = null, string? treeTitle = null, short? trackNumber = null, TimeSpan? duration = null,
			OutputDiscData? disc = null, OutputArtistData? artist = null, OutputGenreData? genre = null, Rating? rating = null, int? bitRate = null,
			DateTimeOffset? lastPlaybackTime = null, int? playbacksCount = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(title, treeTitle, trackNumber, duration, rating, bitRate, lastPlaybackTime, playbacksCount, deleteDate, deleteComment)
		{
			Id = id;
			Disc = disc;
			Artist = artist;
			Genre = genre;
		}
	}
}
