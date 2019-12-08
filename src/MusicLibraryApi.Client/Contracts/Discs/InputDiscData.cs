using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class InputDiscData : BasicDiscData
	{
		[DataMember(Name = "folderId")]
		public int FolderId { get; set; }

		public InputDiscData(int folderId, int? year, string title, string? treeTitle, string albumTitle,
			string? albumId = null, int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(year, title, treeTitle, albumTitle, albumId, albumOrder, deleteDate, deleteComment)
		{
			FolderId = folderId;
		}
	}
}
