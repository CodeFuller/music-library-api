using System;
using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryQuery : ObjectGraphType
	{
		public MusicLibraryQuery(IServiceAccessor serviceAccessor)
		{
			FieldAsync<ListGraphType<GenreType>>(
				"genres",
				resolve: async context => await serviceAccessor.GenresService.GetAllGenres(context.CancellationToken));

			FieldAsync<GenreType>(
				"genre",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var genreId = context.GetArgument<int>("id");
					return await serviceAccessor.GenresService.GetGenre(genreId, context.CancellationToken);
				});

			FieldAsync<ListGraphType<ArtistType>>(
				"artists",
				resolve: async context => await serviceAccessor.ArtistsService.GetAllArtists(context.CancellationToken));

			FieldAsync<ArtistType>(
				"artist",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var artistId = context.GetArgument<int>("id");
					return await serviceAccessor.ArtistsService.GetArtist(artistId, context.CancellationToken);
				});

			FieldAsync<FolderType>(
				"folder",
				arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "folderId" }),
				resolve: async context =>
				{
					var folderId = context.GetArgument<int?>("folderId");
					return await serviceAccessor.FoldersService.GetFolder(folderId, context.CancellationToken);
				});

			FieldAsync<ListGraphType<DiscType>>(
				"discs",
				resolve: async context => await serviceAccessor.DiscsService.GetAllDiscs(context.CancellationToken));

			FieldAsync<DiscType>(
				"disc",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var discId = context.GetArgument<int>("id");
					return await serviceAccessor.DiscsService.GetDisc(discId, context.CancellationToken);
				});

			FieldAsync<ListGraphType<SongType>>(
				"songs",
				resolve: async context => await serviceAccessor.SongsService.GetAllSongs(context.CancellationToken));

			FieldAsync<SongType>(
				"song",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var songId = context.GetArgument<int>("id");
					return await serviceAccessor.SongsService.GetSong(songId, context.CancellationToken);
				});

			// This 'error' field was added for IT purpose.
			// It is required for testing of error handling middleware that hides internal sensitive exceptions.
			Field<StringGraphType>(
				"error",
				resolve: context => throw new InvalidOperationException("Some internal sensitive information goes here"));
		}
	}
}
