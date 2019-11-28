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
					var newFolderId = await serviceAccessor.FoldersService.CreateFolder(folderInput.ParentFolderId, folderInput.GetFolderName(), context.CancellationToken);
					return new CreateFolderResult(newFolderId);
				});

			FieldAsync<CreateDiscResultType>(
				"createDisc",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<DiscInputType>> { Name = "disc" },
					new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "folderId" }),
				resolve: async context =>
				{
					var folderId = context.GetArgument<int>("folderId");
					var discInput = context.GetArgument<DiscInput>("disc");
					var disc = discInput.ToModel();

					var newDiscId = await serviceAccessor.DiscsService.CreateDisc(folderId, disc, context.CancellationToken);
					return new CreateDiscResult(newDiscId);
				});
		}
	}
}
