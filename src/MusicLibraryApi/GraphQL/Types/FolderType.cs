using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class FolderType : ObjectGraphType<Folder>
	{
		public FolderType()
		{
			Field(x => x.Id);
			Field(x => x.Name);
			Field<IntGraphType>("parentFolderId", resolve: context => context.Source.ParentFolder?.Id);
		}
	}
}
