using GraphQL;
using GraphQL.Types;
using GraphQL.Upload.AspNetCore;
using Microsoft.AspNetCore.Http;
using MusicLibraryApi.GraphQL.Types.Input;
using MusicLibraryApi.GraphQL.Types.Output;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryMutation : ObjectGraphType
	{
		public MusicLibraryMutation(IServiceAccessor serviceAccessor)
		{
			FieldAsync<NonNullGraphType<CreateGenreResultType>>(
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

			FieldAsync<NonNullGraphType<CreateArtistResultType>>(
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

			FieldAsync<NonNullGraphType<CreateFolderResultType>>(
				"createFolder",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<FolderInputType>> { Name = "folder" }),
				resolve: async context =>
				{
					var folderInput = context.GetArgument<FolderInput>("folder");
					var folder = folderInput.ToModel();

					var newFolderId = await serviceAccessor.FoldersService.CreateFolder(folder, context.CancellationToken);
					return new CreateFolderResult(newFolderId);
				});

			FieldAsync<NonNullGraphType<CreateDiscResultType>>(
				"createDisc",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<DiscInputType>> { Name = "disc" },
					new QueryArgument<UploadGraphType> { Name = "coverFile" }),
				resolve: async context =>
				{
					var discInput = context.GetArgument<DiscInput>("disc");
					var disc = discInput.ToModel();

					var file = context.GetArgument<IFormFile>("coverFile");

					int newDiscId;

					if (file != null)
					{
						await using var contentStream = file.OpenReadStream();
						newDiscId = await serviceAccessor.DiscsService.CreateDisc(disc, contentStream, context.CancellationToken);
					}
					else
					{
						newDiscId = await serviceAccessor.DiscsService.CreateDisc(disc, context.CancellationToken);
					}

					return new CreateDiscResult(newDiscId);
				});

			FieldAsync<NonNullGraphType<CreateSongResultType>>(
				"createSong",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<SongInputType>> { Name = "song" },
					new QueryArgument<NonNullGraphType<UploadGraphType>> { Name = "songFile" }),
				resolve: async context =>
				{
					var songInput = context.GetArgument<SongInput>("song");
					var song = songInput.ToModel();

					var file = context.GetArgument<IFormFile>("songFile");
					await using var contentStream = file.OpenReadStream();

					var newSongId = await serviceAccessor.SongsService.CreateSong(song, contentStream, context.CancellationToken);
					return new CreateSongResult(newSongId);
				});

			FieldAsync<NonNullGraphType<CreateSongResultType>>(
				"createDeletedSong",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<SongInputType>> { Name = "song" }),
				resolve: async context =>
				{
					var songInput = context.GetArgument<SongInput>("song");
					var song = songInput.ToModel();

					var newSongId = await serviceAccessor.SongsService.CreateDeletedSong(song, context.CancellationToken);
					return new CreateSongResult(newSongId);
				});

			FieldAsync<NonNullGraphType<AddPlaybackResultType>>(
				"addSongPlayback",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<PlaybackInputType>> { Name = "playback" }),
				resolve: async context =>
				{
					var playbackInput = context.GetArgument<PlaybackInput>("playback");
					var playback = playbackInput.ToModel();

					var newPlaybackId = await serviceAccessor.PlaybacksService.CreatePlayback(playback, context.CancellationToken);
					return new AddPlaybackResult(newPlaybackId);
				});
		}
	}
}
