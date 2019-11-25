using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Artists
{
	[DataContract]
	public class OutputArtistData : BasicArtistData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputArtistData(int? id, string? name)
			: base(name)
		{
			Id = id;
		}
	}
}
