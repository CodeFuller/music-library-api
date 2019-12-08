using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class GenreType : ObjectGraphType<Genre>
	{
		public GenreType(IContextServiceAccessor serviceAccessor)
		{
			Field(x => x.Id);
			Field(x => x.Name);
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<SongType>>>>(
				"songs",
				resolve: async context => await serviceAccessor.SongsService.GetGenreSongs(context.Source.Id, context.CancellationToken));
		}
	}
}
