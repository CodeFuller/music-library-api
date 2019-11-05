using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types
{
	public class FolderInputType : InputObjectGraphType
	{
		public FolderInputType()
		{
			Name = "FolderInput";
			Field<NonNullGraphType<StringGraphType>>("name");
		}
	}
}
