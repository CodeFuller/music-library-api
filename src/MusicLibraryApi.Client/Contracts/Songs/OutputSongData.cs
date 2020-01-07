using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Playbacks;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class OutputSongData : BasicSongData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		[DataMember(Name = "disc")]
		public OutputDiscData? Disc { get; set; }

		[DataMember(Name = "artist")]
		public OutputArtistData? Artist { get; set; }

		[DataMember(Name = "genre")]
		public OutputGenreData? Genre { get; set; }

		[DataMember(Name = "size")]
		public long? Size { get; set; }

		[DataMember(Name = "lastPlaybackTime")]
		public DateTimeOffset? LastPlaybackTime { get; set; }

		[DataMember(Name = "playbacksCount")]
		public int? PlaybacksCount { get; set; }

		[DataMember(Name = "playbacks")]
		public IReadOnlyCollection<OutputPlaybackData>? Playbacks { get; set; }
	}
}
