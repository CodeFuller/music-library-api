using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class RatingSongsNumberType : ObjectGraphType<(Rating?, int)>
	{
		public RatingSongsNumberType()
		{
			Field<RatingEnumType>("rating", resolve: context => context.Source.Item1);
			Field<NonNullGraphType<IntGraphType>>("songsNumber", resolve: context => context.Source.Item2);
		}
	}
}
