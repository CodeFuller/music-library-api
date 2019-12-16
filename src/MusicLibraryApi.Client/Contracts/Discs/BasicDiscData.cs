using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public abstract class BasicDiscData
	{
		[DataMember(Name = "year")]
		public int? Year { get; set; }

		[DataMember(Name = "title")]
		public string? Title { get; set; }

		[DataMember(Name = "treeTitle")]
		public string? TreeTitle { get; set; }

		[DataMember(Name = "albumTitle")]
		public string? AlbumTitle { get; set; }

		[DataMember(Name = "albumId")]
		public string? AlbumId { get; set; }

		[DataMember(Name = "albumOrder")]
		public int? AlbumOrder { get; set; }

		[DataMember(Name = "deleteDate")]
		public DateTimeOffset? DeleteDate { get; set; }

		[DataMember(Name = "deleteComment")]
		public string? DeleteComment { get; set; }
	}
}
