using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.GraphQL.Types.Input;
using MusicLibraryApi.GraphQL.Types.Output;
using MusicLibraryApi.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryMutation : ObjectGraphType
	{
		public MusicLibraryMutation(IContextRepositoryAccessor repositoryAccessor, ILogger<MusicLibraryMutation> logger)
		{
			FieldAsync<CreateGenreResultType>(
				"createGenre",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<GenreInputType>> { Name = "genre" }),
				resolve: async context =>
				{
					var genreInput = context.GetArgument<GenreInput>("genre");
					var genre = genreInput.ToModel();

					try
					{
						var newGenreId = await repositoryAccessor.GenresRepository.AddGenre(genre, context.CancellationToken);
						return new CreateGenreResult(newGenreId);
					}
					catch (DuplicateKeyException e)
					{
						logger.LogError(e, "Genre {GenreName} already exists", genre.Name);
						throw new ExecutionError(Invariant($"Genre '{genre.Name}' already exists"));
					}
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

					try
					{
						var newDiscId = await repositoryAccessor.DiscsRepository.AddDisc(folderId, disc, context.CancellationToken);
						return new CreateDiscResult(newDiscId);
					}
					catch (NotFoundException e)
					{
						logger.LogError(e, "The folder with id of {FolderId} does not exist", folderId);
						throw new ExecutionError(Invariant($"The folder with id of '{folderId}' does not exist"));
					}
				});
		}
	}
}
