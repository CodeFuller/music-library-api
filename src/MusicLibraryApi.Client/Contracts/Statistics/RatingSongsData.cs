using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Statistics
{
	[DataContract]
	public class RatingSongsData
	{
		[DataMember(Name = "rating")]
		public Rating? Rating { get; set; }

		[DataMember(Name = "songsNumber")]
		public int? SongsNumber { get; set; }

		public RatingSongsData(Rating? rating, int? songsNumber)
		{
			Rating = rating;
			SongsNumber = songsNumber;
		}
	}
}
