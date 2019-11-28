using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class FolderInputType : InputObjectGraphType
	{
		public FolderInputType()
		{
			Name = "FolderInput";
			Field<NonNullGraphType<StringGraphType>>("name");
			Field<IdGraphType>("parentFolderId");
		}
	}
}
