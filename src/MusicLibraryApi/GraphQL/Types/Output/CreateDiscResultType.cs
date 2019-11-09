using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateDiscResultType : ObjectGraphType<CreateDiscResult>
	{
		public CreateDiscResultType()
		{
			Field(x => x.NewDiscId);
		}
	}
}
