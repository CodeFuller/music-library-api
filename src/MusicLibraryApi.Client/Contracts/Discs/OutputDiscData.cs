using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class OutputDiscData : BasicDiscData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputDiscData(int? id, int? year, string? title, string? albumTitle = null, int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(year, title, albumTitle, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
