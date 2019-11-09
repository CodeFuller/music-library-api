using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryQuery : ObjectGraphType
	{
		public MusicLibraryQuery(IContextRepositoryAccessor repositoryAccessor)
		{
			FieldAsync<ListGraphType<GenreType>>(
				"genres",
				resolve: async context => await repositoryAccessor.GenresRepository.GetAllGenres(context.CancellationToken));

			FieldAsync<DiscType>(
				"disc",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
				resolve: async context => await repositoryAccessor.DiscsRepository.GetDisc(context.GetArgument<int>("id"), context.CancellationToken));

			FieldAsync<ListGraphType<DiscType>>(
				"discs",
				resolve: async context => await repositoryAccessor.DiscsRepository.GetAllDiscs(context.CancellationToken));
		}
	}
}
