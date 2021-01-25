using GraphQL.DataLoader;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class SongType : ObjectGraphType<Song>
	{
		public SongType(IServiceAccessor serviceAccessor, IDataLoaderContextAccessor dataLoader)
		{
			Field(x => x.Id);
			Field(x => x.Title);
			Field(x => x.TreeTitle);
			Field(x => x.TrackNumber, nullable: true);
			Field(x => x.Duration);
			Field<NonNullGraphType<DiscType>>("disc", resolve: context =>
			{
				var discsService = serviceAccessor.DiscsService;
				var loader = dataLoader.Context.GetOrAddBatchLoader<int, Disc>("GetDiscsById", discsService.GetDiscs);
				return loader.LoadAsync(context.Source.DiscId);
			});
			Field<ArtistType>("artist", resolve: context =>
			{
				if (context.Source.ArtistId == null)
				{
					return null;
				}

				var artistsService = serviceAccessor.ArtistsService;
				var loader = dataLoader.Context.GetOrAddBatchLoader<int, Artist>("GetArtistsById", artistsService.GetArtists);
				return loader.LoadAsync(context.Source.ArtistId.Value);
			});
			Field<GenreType>("genre", resolve: context =>
			{
				if (context.Source.GenreId == null)
				{
					return null;
				}

				var genresService = serviceAccessor.GenresService;
				var loader = dataLoader.Context.GetOrAddBatchLoader<int, Genre>("GetGenresById", genresService.GetGenres);
				return loader.LoadAsync(context.Source.GenreId.Value);
			});
			Field<RatingEnumType>("rating");
			Field(x => x.BitRate, nullable: true);
			Field(x => x.Size, nullable: true);
			Field(x => x.LastPlaybackTime, nullable: true);
			Field(x => x.PlaybacksCount);
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<PlaybackType>>>>(
				"playbacks",
				resolve: context =>
				{
					var playbacksService = serviceAccessor.PlaybacksService;
					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Playback>("GetPlaybacksBySongIds", playbacksService.GetPlaybacksBySongIds);
					return loader.LoadAsync(context.Source.Id);
				});
			Field(x => x.DeleteDate, nullable: true);
			Field(x => x.DeleteComment, nullable: true);
		}
	}
}
