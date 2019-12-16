using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Genres
{
	[DataContract]
	public class OutputGenreData : BasicGenreData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; set; }
	}
}
