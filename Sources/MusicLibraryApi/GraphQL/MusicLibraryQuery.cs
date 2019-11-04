using System.Threading;
using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryQuery : ObjectGraphType
	{
		public MusicLibraryQuery(IContextRepositoryAccessor repositoryAccessor)
		{
			Field<ListGraphType<GenreType>>(
				"genres",
				resolve: context => repositoryAccessor.GenresRepository.GetAllGenres(CancellationToken.None));

			Field<DiscType>(
				"disc",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
				resolve: context => repositoryAccessor.DiscsRepository.GetDisc(context.GetArgument<int>("id"), CancellationToken.None));

			Field<ListGraphType<DiscType>>(
				"discs",
				resolve: context => repositoryAccessor.DiscsRepository.GetAllDiscs(CancellationToken.None));
		}
	}
}
