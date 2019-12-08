using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public class OutputArtistData : BasicArtistData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; }

		public OutputArtistData(int? id = null, string? name = null, IReadOnlyCollection<OutputSongData>? songs = null)
			: base(name)
		{
			Id = id;
			Songs = songs;
		}
	}
}
