using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class InputDiscData : BasicDiscData
	{
		public InputDiscData(int? year, string title, string albumTitle, int? albumOrder, DateTimeOffset? deleteDate, string deleteComment)
			: base(year, title, albumTitle, albumOrder, deleteDate, deleteComment)
		{
		}
	}
}
