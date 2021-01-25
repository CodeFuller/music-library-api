using System;
using GraphQL;
using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryQuery : ObjectGraphType
	{
		public MusicLibraryQuery(IServiceAccessor serviceAccessor)
		{
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<GenreType>>>>(
				"genres",
				resolve: async context => await serviceAccessor.GenresService.GetAllGenres(context.CancellationToken));

			FieldAsync<NonNullGraphType<GenreType>>(
				"genre",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var genreId = context.GetArgument<int>("id");
					return await serviceAccessor.GenresService.GetGenre(genreId, context.CancellationToken);
				});

			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<ArtistType>>>>(
				"artists",
				resolve: async context => await serviceAccessor.ArtistsService.GetAllArtists(context.CancellationToken));

			FieldAsync<NonNullGraphType<ArtistType>>(
				"artist",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var artistId = context.GetArgument<int>("id");
					return await serviceAccessor.ArtistsService.GetArtist(artistId, context.CancellationToken);
				});

			FieldAsync<NonNullGraphType<FolderType>>(
				"folder",
				arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "folderId" }),
				resolve: async context =>
				{
					var folderId = context.GetArgument<int?>("folderId");
					return await serviceAccessor.FoldersService.GetFolder(folderId, context.CancellationToken);
				});

			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<DiscType>>>>(
				"discs",
				resolve: async context => await serviceAccessor.DiscsService.GetAllDiscs(context.CancellationToken));

			FieldAsync<NonNullGraphType<DiscType>>(
				"disc",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var discId = context.GetArgument<int>("id");
					return await serviceAccessor.DiscsService.GetDisc(discId, context.CancellationToken);
				});

			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<SongType>>>>(
				"songs",
				resolve: async context => await serviceAccessor.SongsService.GetAllSongs(context.CancellationToken));

			FieldAsync<NonNullGraphType<SongType>>(
				"song",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var songId = context.GetArgument<int>("id");
					return await serviceAccessor.SongsService.GetSong(songId, context.CancellationToken);
				});

			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<PlaybackType>>>>(
				"playbacks",
				resolve: async context => await serviceAccessor.PlaybacksService.GetAllPlaybacks(context.CancellationToken));

			FieldAsync<NonNullGraphType<PlaybackType>>(
				"playback",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var playbackId = context.GetArgument<int>("id");
					return await serviceAccessor.PlaybacksService.GetPlayback(playbackId, context.CancellationToken);
				});

			Field<NonNullGraphType<StatisticsType>>(
				"statistics",
				resolve: context => new object());

			// This 'error' field was added for IT purpose.
			// It is required for testing of error handling middleware that hides internal sensitive exceptions.
			Field<StringGraphType>(
				"error",
				resolve: context => throw new InvalidOperationException("Some internal sensitive information goes here"));
		}
	}
}
