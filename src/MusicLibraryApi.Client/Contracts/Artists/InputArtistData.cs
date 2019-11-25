using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public class InputArtistData : BasicArtistData
	{
		public InputArtistData(string name)
			: base(name)
		{
		}
	}
}
