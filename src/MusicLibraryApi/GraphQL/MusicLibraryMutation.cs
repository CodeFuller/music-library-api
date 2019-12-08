using System;
using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types.Input;
using MusicLibraryApi.GraphQL.Types.Output;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryMutation : ObjectGraphType
	{
		public MusicLibraryMutation(IContextServiceAccessor serviceAccessor)
		{
			FieldAsync<CreateGenreResultType>(
				"createGenre",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<GenreInputType>> { Name = "genre" }),
				resolve: async context =>
				{
					var genreInput = context.GetArgument<GenreInput>("genre");
					var genre = genreInput.ToModel();

					var newGenreId = await serviceAccessor.GenresService.CreateGenre(genre, context.CancellationToken);
					return new CreateGenreResult(newGenreId);
				});

			FieldAsync<CreateArtistResultType>(
				"createArtist",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<ArtistInputType>> { Name = "artist" }),
				resolve: async context =>
				{
					var artistInput = context.GetArgument<ArtistInput>("artist");
					var artist = artistInput.ToModel();

					var newArtistId = await serviceAccessor.ArtistsService.CreateArtist(artist, context.CancellationToken);
					return new CreateArtistResult(newArtistId);
				});

			FieldAsync<CreateFolderResultType>(
				"createFolder",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<FolderInputType>> { Name = "folder" }),
				resolve: async context =>
				{
					var folderInput = context.GetArgument<FolderInput>("folder");

					if (folderInput.ParentFolderId == null)
					{
						throw new InvalidOperationException("Parent folder id is not set");
					}

					var newFolderId = await serviceAccessor.FoldersService.CreateFolder(folderInput.ParentFolderId.Value, folderInput.GetFolderName(), context.CancellationToken);
					return new CreateFolderResult(newFolderId);
				});

			FieldAsync<CreateDiscResultType>(
				"createDisc",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<DiscInputType>> { Name = "disc" }),
				resolve: async context =>
				{
					var discInput = context.GetArgument<DiscInput>("disc");
					var disc = discInput.ToModel();

					var newDiscId = await serviceAccessor.DiscsService.CreateDisc(discInput.FolderId, disc, context.CancellationToken);
					return new CreateDiscResult(newDiscId);
				});

			FieldAsync<CreateSongResultType>(
				"createSong",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<SongInputType>> { Name = "song" }),
				resolve: async context =>
				{
					var songInput = context.GetArgument<SongInput>("song");
					var song = songInput.ToModel();

					if (songInput.DiscId == null)
					{
						throw new InvalidOperationException("Disc for the song is not set");
					}

					var newSongId = await serviceAccessor.SongsService.CreateSong(songInput.DiscId.Value, songInput.ArtistId, songInput.GenreId, song, context.CancellationToken);
					return new CreateSongResult(newSongId);
				});
		}
	}
}
