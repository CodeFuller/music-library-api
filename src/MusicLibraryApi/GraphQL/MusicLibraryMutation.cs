using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types.Input;
using MusicLibraryApi.GraphQL.Types.Output;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryMutation : ObjectGraphType
	{
		public MusicLibraryMutation(IContextRepositoryAccessor repositoryAccessor)
		{
			FieldAsync<CreateGenreResultType>(
				"createGenre",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<GenreInputType>> { Name = "genre" }),
				resolve: async context =>
				{
					var genreInput = context.GetArgument<GenreInput>("genre");
					var genre = genreInput.ToModel();
					var newGenreId = await repositoryAccessor.GenresRepository.AddGenre(genre, context.CancellationToken);
					return new CreateGenreResult(newGenreId);
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
					var newDiscId = await repositoryAccessor.DiscsRepository.AddDisc(folderId, disc, context.CancellationToken);

					return new CreateDiscResult(newDiscId);
				});
		}
	}
}
