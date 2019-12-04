using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class OutputDiscData : BasicDiscData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputDiscData(int? id, int? year, string? title, string? albumTitle, string? albumId = null,
			int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(year, title, albumTitle, albumId, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
