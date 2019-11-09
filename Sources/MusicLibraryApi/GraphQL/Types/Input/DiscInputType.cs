using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class DiscInputType : InputObjectGraphType
	{
		public DiscInputType()
		{
			Name = "DiscInput";

			Field<IntGraphType>("year");
			Field<NonNullGraphType<StringGraphType>>("title");
			Field<StringGraphType>("albumTitle");
			Field<IntGraphType>("albumOrder");
			Field<DateTimeOffsetGraphType>("deleteDate");
			Field<StringGraphType>("deleteComment");
		}
	}
}
