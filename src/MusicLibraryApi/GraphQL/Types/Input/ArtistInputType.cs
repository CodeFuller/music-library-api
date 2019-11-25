using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class ArtistInputType : InputObjectGraphType<ArtistInput>
	{
		public ArtistInputType()
		{
			Name = "ArtistInput";
			Field<NonNullGraphType<StringGraphType>>("name");
		}
	}
}
