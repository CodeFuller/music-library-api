using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class OutputDiscData : BasicDiscData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "folder")]
		public OutputFolderData? Folder { get; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; }

		public OutputDiscData(int? id = null, int? year = null, string? title = null, string? treeTitle = null, string? albumTitle = null, string? albumId = null,
			int? albumOrder = null, OutputFolderData? folder = null, IReadOnlyCollection<OutputSongData>? songs = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(year, title, treeTitle, albumTitle, albumId, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
			Folder = folder;
			Songs = songs;
		}
	}
}
