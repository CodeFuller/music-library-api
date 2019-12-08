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
			Field(name: "subfolders", type: typeof(ListGraphType<NonNullGraphType<FolderType>>), resolve: context => context.Source.Subfolders);
			Field(name: "discs", type: typeof(ListGraphType<NonNullGraphType<DiscType>>), resolve: context => context.Source.Discs);
		}
	}
}
