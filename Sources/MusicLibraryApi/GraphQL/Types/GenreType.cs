using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class GenreType : ObjectGraphType<Genre>
	{
		public GenreType()
		{
			Field(x => x.Id);
			Field(x => x.Name);
		}
	}
}
