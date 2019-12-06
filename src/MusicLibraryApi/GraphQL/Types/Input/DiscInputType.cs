using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class DiscInputType : InputObjectGraphType<DiscInput>
	{
		public DiscInputType()
		{
			Name = "DiscInput";

			Field<IntGraphType>("year");
			Field<NonNullGraphType<StringGraphType>>("title");
			Field<NonNullGraphType<StringGraphType>>("treeTitle");
			Field<NonNullGraphType<StringGraphType>>("albumTitle");
			Field<StringGraphType>("albumId");
			Field<IntGraphType>("albumOrder");
			Field<DateTimeOffsetGraphType>("deleteDate");
			Field<StringGraphType>("deleteComment");
		}
	}
}
