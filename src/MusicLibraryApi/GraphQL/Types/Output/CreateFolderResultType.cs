using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateFolderResultType : ObjectGraphType<CreateFolderResult>
	{
		public CreateFolderResultType()
		{
			Field(x => x.NewFolderId);
		}
	}
}
