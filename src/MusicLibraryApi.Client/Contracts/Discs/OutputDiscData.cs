using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class OutputDiscData : BasicDiscData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputDiscData(int? id, int? year, string? title, string? albumTitle, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
			: base(year, title, albumTitle, albumOrder, deleteDate, deleteComment)
		{
			Id = id;
		}
	}
}
