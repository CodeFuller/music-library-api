using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Contracts.Genres
{
	[DataContract]
	public class OutputGenreData : BasicGenreData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "songs")]
		public IReadOnlyCollection<OutputSongData>? Songs { get; }

		public OutputGenreData(int? id, string? name, IReadOnlyCollection<OutputSongData>? songs = null)
			: base(name)
		{
			Id = id;
			Songs = songs;
		}
	}
}
