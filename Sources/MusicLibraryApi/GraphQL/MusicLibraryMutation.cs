using System.Threading;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryMutation : ObjectGraphType
	{
		public MusicLibraryMutation(IContextRepositoryAccessor repositoryAccessor)
		{
			Field<GenreType>(
				"createGenre",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<GenreInputType>> { Name = "genre" }),
				resolve: context =>
				{
					var genre = context.GetArgument<Genre>("genre");
					return repositoryAccessor.GenresRepository.AddGenre(genre, CancellationToken.None);
				});

			Field<DiscType>(
				"createDisc",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<DiscInputType>> { Name = "disc" },
					new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "folderId" }),
				resolve: context =>
				{
					var folderId = context.GetArgument<int>("folderId");
					var disc = context.GetArgument<Disc>("disc");
					return repositoryAccessor.DiscsRepository.AddDisc(folderId, disc, CancellationToken.None);
				});
		}
	}
}
