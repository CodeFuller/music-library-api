using GraphQL.DataLoader;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class FolderType : ObjectGraphType<Folder>
	{
		public FolderType(IServiceAccessor serviceAccessor, IDataLoaderContextAccessor dataLoader)
		{
			Field(x => x.Id);
			Field(x => x.Name);
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<FolderType>>>>(
				"subfolders",
				resolve: context =>
				{
					var foldersService = serviceAccessor.FoldersService;
					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Folder>("GetSubfoldersByFolderIds", foldersService.GetSubfoldersByFolderIds);
					return loader.LoadAsync(context.Source.Id);
				});
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<DiscType>>>>(
				"discs",
				arguments: new QueryArguments(new QueryArgument<BooleanGraphType> { Name = "includeDeletedDiscs" }),
				resolve: context =>
				{
					var includeDeletedDiscs = context.GetArgument<bool?>("includeDeletedDiscs") ?? false;
					var foldersService = serviceAccessor.DiscsService;
					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Disc>("GetDiscsByFolderIds", (ids, cnt) => foldersService.GetDiscsByFolderIds(ids, includeDeletedDiscs, cnt));
					return loader.LoadAsync(context.Source.Id);
				});
		}
	}
}
