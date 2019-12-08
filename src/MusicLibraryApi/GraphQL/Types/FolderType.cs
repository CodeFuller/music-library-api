using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class FolderType : ObjectGraphType<Folder>
	{
		public FolderType(IContextServiceAccessor serviceAccessor)
		{
			Field(x => x.Id);
			Field(x => x.Name);
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<FolderType>>>>(
				"subfolders",
				resolve: async context => await serviceAccessor.FoldersService.GetFolderSubfolders(context.Source.Id, context.CancellationToken));
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<DiscType>>>>(
				"discs",
				arguments: new QueryArguments(
					new QueryArgument<BooleanGraphType> { Name = "includeDeletedDiscs" }),
				resolve: async context =>
				{
					var includeDeletedDiscs = context.GetArgument<bool?>("includeDeletedDiscs") ?? false;
					return await serviceAccessor.DiscsService.GetFolderDiscs(context.Source.Id, includeDeletedDiscs, context.CancellationToken);
				});
		}
	}
}
