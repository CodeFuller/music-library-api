using GraphQL.DataLoader;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class ArtistType : ObjectGraphType<Artist>
	{
		public ArtistType(IContextServiceAccessor serviceAccessor, IDataLoaderContextAccessor dataLoader)
		{
			Field(x => x.Id);
			Field(x => x.Name);
			Field<ListGraphType<SongType>>(
				"songs",
				resolve: context =>
				{
					var songsService = serviceAccessor.SongsService;
					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Song>("GetSongsByArtistIds", songsService.GetSongsByArtistIds);
					return loader.LoadAsync(context.Source.Id);
				});
		}
	}
}
