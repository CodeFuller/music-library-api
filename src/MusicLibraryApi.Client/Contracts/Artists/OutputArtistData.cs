using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public class OutputArtistData : BasicArtistData
	{
		[DataMember(Name = "id")]
		public int? Id { get; set; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; set; }
	}
}
