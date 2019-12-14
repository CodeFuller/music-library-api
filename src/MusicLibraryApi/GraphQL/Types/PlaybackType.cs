using GraphQL.DataLoader;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class PlaybackType : ObjectGraphType<Playback>
	{
		public PlaybackType(IServiceAccessor serviceAccessor, IDataLoaderContextAccessor dataLoader)
		{
			Field(x => x.Id);
			Field(x => x.PlaybackTime);
			Field<NonNullGraphType<SongType>>("song", resolve: context =>
			{
				var songsService = serviceAccessor.SongsService;
				var loader = dataLoader.Context.GetOrAddBatchLoader<int, Song>("GetSongs", songsService.GetSongs);
				return loader.LoadAsync(context.Source.SongId);
			});
		}
	}
}
