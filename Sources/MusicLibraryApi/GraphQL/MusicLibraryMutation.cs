using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.GraphQL.Types;
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
					var genre = context.GetArgument<Genre>("genre");
					var newGenreId = await repositoryAccessor.GenresRepository.AddGenre(genre, context.CancellationToken);
					return new CreateGenreResult(newGenreId);
				});

			FieldAsync<CreateDiscResultType>(
				"createDisc",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<DiscInputType>> { Name = "disc" },
					new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "folderId" }),
				resolve: async context =>
				{
					var folderId = context.GetArgument<int>("folderId");
					var disc = context.GetArgument<Disc>("disc");
					var newDiscId = await repositoryAccessor.DiscsRepository.AddDisc(folderId, disc, context.CancellationToken);

					return new CreateDiscResult(newDiscId);
				});
		}
	}
}
