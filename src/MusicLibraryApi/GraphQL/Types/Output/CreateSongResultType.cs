using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateSongResultType : ObjectGraphType<CreateSongResult>
	{
		public CreateSongResultType()
		{
			Field(x => x.NewSongId);
		}
	}
}
