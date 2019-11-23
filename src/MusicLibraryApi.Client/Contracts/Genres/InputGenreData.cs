using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Genres
{
	[DataContract]
	public class InputGenreData : BasicGenreData
	{
		public InputGenreData(string name)
			: base(name)
		{
		}
	}
}
