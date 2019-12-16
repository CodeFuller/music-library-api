using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Genres
{
	[DataContract]
	public class CreateGenreOutputData
	{
		[DataMember(Name = "newGenreId")]
		public int? NewGenreId { get; set; }
	}
}
