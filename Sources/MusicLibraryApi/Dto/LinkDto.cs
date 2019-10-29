using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Dto
{
	[DataContract]
	public class LinkDto
	{
		[DataMember(Name = "rel")]
		public string Relation { get; set; }

		[DataMember(Name = "href")]
		public Uri Uri { get; set; }

		public static LinkDto Self(Uri uri)
		{
			return new LinkDto
			{
				Relation = "self",
				Uri = uri,
			};
		}
	}
}
