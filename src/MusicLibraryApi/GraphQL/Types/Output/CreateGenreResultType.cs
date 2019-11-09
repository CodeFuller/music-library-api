using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateGenreResultType : ObjectGraphType<CreateGenreResult>
	{
		public CreateGenreResultType()
		{
			Field(x => x.NewGenreId);
		}
	}
}
