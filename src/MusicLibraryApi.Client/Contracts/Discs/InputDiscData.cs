using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class InputDiscData : BasicDiscData
	{
		public InputDiscData(int? year, string title, string? albumTitle = null, int? albumOrder = null, DateTimeOffset? deleteDate = null, string? deleteComment = null)
			: base(year, title, albumTitle, albumOrder, deleteDate, deleteComment)
		{
		}
	}
}
