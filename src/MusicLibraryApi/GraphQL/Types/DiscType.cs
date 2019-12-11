using GraphQL.DataLoader;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class DiscType : ObjectGraphType<Disc>
	{
		public DiscType(IContextServiceAccessor serviceAccessor, IDataLoaderContextAccessor dataLoader)
		{
			Field(x => x.Id);
			Field(x => x.Year, nullable: true);
			Field(x => x.Title);
			Field(x => x.TreeTitle);
			Field(x => x.AlbumTitle);
			Field(x => x.AlbumId, nullable: true);
			Field(x => x.AlbumOrder, nullable: true);
			Field<FolderType>("folder", resolve: context => context.Source.Folder);
			Field(x => x.DeleteDate, nullable: true);
			Field(x => x.DeleteComment, nullable: true);
			Field<ListGraphType<SongType>>(
				"songs",
				resolve: context =>
				{
					var songsService = serviceAccessor.SongsService;
					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Song>("GetSongsByDiscIds", songsService.GetSongsByDiscIds);
					return loader.LoadAsync(context.Source.Id);
				});
		}
	}
}
