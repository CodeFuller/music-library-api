using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class ArtistType : ObjectGraphType<Artist>
	{
		public ArtistType()
		{
			Field(x => x.Id);
			Field(x => x.Name);
		}
	}
}
