using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class SongType : ObjectGraphType<Song>
	{
		public SongType(IContextRepositoryAccessor repositoryAccessor)
		{
			Field(x => x.Id);
			Field(x => x.Title);
			Field(x => x.Duration, true);

			// Current implementation of GraphQL can not currently automatically infer short type.
			Field<IntGraphType>("trackNumber", resolve: context => context.Source.TrackNumber);
		}
	}
}
