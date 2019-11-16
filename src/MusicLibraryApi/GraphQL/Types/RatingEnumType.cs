using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class RatingEnumType : EnumerationGraphType<Rating>
	{
		public RatingEnumType()
		{
			Name = "Rating";
			Description = "The song rating";
		}
	}
}
