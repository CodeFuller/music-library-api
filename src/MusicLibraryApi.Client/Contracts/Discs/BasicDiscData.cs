using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public abstract class BasicDiscData
	{
		[DataMember(Name = "year")]
		public int? Year { get; }

		[DataMember(Name = "title")]
		public string? Title { get; }

		[DataMember(Name = "treeTitle")]
		public string? TreeTitle { get; }

		[DataMember(Name = "albumTitle")]
		public string? AlbumTitle { get; }

		[DataMember(Name = "albumId")]
		public string? AlbumId { get; }

		[DataMember(Name = "albumOrder")]
		public int? AlbumOrder { get; }

		[DataMember(Name = "deleteDate")]
		public DateTimeOffset? DeleteDate { get; }

		[DataMember(Name = "deleteComment")]
		public string? DeleteComment { get; }

		protected BasicDiscData(int? year, string? title, string? treeTitle, string? albumTitle, string? albumId, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
		{
			Year = year;
			Title = title;
			TreeTitle = treeTitle;
			AlbumTitle = albumTitle;
			AlbumId = albumId;
			AlbumOrder = albumOrder;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}
	}
}
