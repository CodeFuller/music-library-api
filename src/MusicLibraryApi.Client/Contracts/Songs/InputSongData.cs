using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Songs
{
	[DataContract]
	public class InputSongData : BasicSongData
	{
		[DataMember(Name = "discId")]
		public int DiscId { get; set; }

		[DataMember(Name = "artistId")]
		public int? ArtistId { get; set; }

		[DataMember(Name = "genreId")]
		public int? GenreId { get; set; }
	}
}
