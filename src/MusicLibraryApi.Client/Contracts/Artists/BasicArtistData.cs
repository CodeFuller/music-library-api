using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public abstract class BasicArtistData
	{
		[DataMember(Name = "name")]
		public string? Name { get; set; }
	}
}
