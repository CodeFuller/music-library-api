using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Genres
{
	[DataContract]
	public abstract class BasicGenreData
	{
		[DataMember(Name = "name")]
		public string? Name { get; }

		protected BasicGenreData(string? name)
		{
			Name = name;
		}
	}
}
