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
			Name = "CreateGenreMutation";

			Field<GenreType>(
				"createGenre",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<GenreInputType>> { Name = "genre" }),
				resolve: context =>
				{
					var genre = context.GetArgument<Genre>("genre");
					return repositoryAccessor.GenresRepository.AddGenre(genre, CancellationToken.None);
				});
		}
	}
}
