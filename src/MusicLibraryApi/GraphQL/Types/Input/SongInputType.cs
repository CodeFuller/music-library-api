﻿using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class SongInputType : InputObjectGraphType<SongInput>
	{
		public SongInputType()
		{
			Name = "SongInput";

			Field<NonNullGraphType<IdGraphType>>("discId");
			Field<IdGraphType>("artistId");
			Field<IdGraphType>("genreId");
			Field<NonNullGraphType<StringGraphType>>("title");
			Field<NonNullGraphType<StringGraphType>>("treeTitle");
			Field<IntGraphType>("trackNumber", resolve: context => context.Source.TrackNumber);
			Field<NonNullGraphType<TimeSpanSecondsGraphType>>("duration");
			Field<RatingEnumType>("rating");
			Field<IntGraphType>("bitRate");
			Field<DateTimeOffsetGraphType>("lastPlaybackTime");
			Field<DateTimeOffsetGraphType>("deleteDate");
			Field<StringGraphType>("deleteComment");
		}
	}
}
