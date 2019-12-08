using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class OutputDiscData : BasicDiscData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; }

		public OutputDiscData(int? id, int? year, string? title, string? treeTitle, string? albumTitle, string? albumId = null,
			int? albumOrder = null, IReadOnlyCollection<OutputSongData>? songs = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(year, title, treeTitle, albumTitle, albumId, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
			Songs = songs;
		}
	}
}
