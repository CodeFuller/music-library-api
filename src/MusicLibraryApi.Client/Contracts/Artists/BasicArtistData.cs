using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public abstract class BasicArtistData
	{
		[DataMember(Name = "name")]
		public string? Name { get; }

		protected BasicArtistData(string? name)
		{
			Name = name;
		}
	}
}
