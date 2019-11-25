using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateArtistResultType : ObjectGraphType<CreateArtistResult>
	{
		public CreateArtistResultType()
		{
			Field(x => x.NewArtistId);
		}
	}
}
