using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Genres
{
	[DataContract]
	public class OutputGenreData : BasicGenreData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		public OutputGenreData(int? id, string? name)
			: base(name)
		{
			Id = id;
		}
	}
}
