using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class GenreInputType : InputObjectGraphType
	{
		public GenreInputType()
		{
			Name = "GenreInput";
			Field<NonNullGraphType<StringGraphType>>("name");
		}
	}
}
