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

		[DataMember(Name = "albumTitle")]
		public string? AlbumTitle { get; }

		[DataMember(Name = "albumOrder")]
		public int? AlbumOrder { get; }

		[DataMember(Name = "deleteDate")]
		public DateTimeOffset? DeleteDate { get; }

		[DataMember(Name = "deleteComment")]
		public string? DeleteComment { get; }

		protected BasicDiscData(int? year, string? title, string? albumTitle, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
		{
			Year = year;
			Title = title;
			AlbumTitle = albumTitle;
			AlbumOrder = albumOrder;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}
	}
}
