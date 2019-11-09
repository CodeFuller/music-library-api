using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class GenreInputType : InputObjectGraphType<GenreInput>
	{
		public GenreInputType()
		{
			Name = "GenreInput";
			Field<NonNullGraphType<StringGraphType>>("name");
		}
	}
}
