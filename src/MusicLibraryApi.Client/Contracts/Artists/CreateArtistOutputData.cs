using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public class CreateArtistOutputData
	{
		[DataMember(Name = "newArtistId")]
		public int? NewArtistId { get; }

		public CreateArtistOutputData(int? newArtistId)
		{
			NewArtistId = newArtistId;
		}
	}
}
