using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class RatingSongsNumberType : ObjectGraphType<(Rating? Rating, int SongsNumber)>
	{
		public RatingSongsNumberType()
		{
			Field<RatingEnumType>("rating", resolve: context => context.Source.Rating);
			Field<NonNullGraphType<IntGraphType>>("songsNumber", resolve: context => context.Source.SongsNumber);
		}
	}
}
