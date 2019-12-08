using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class SongType : ObjectGraphType<Song>
	{
		public SongType()
		{
			Field(x => x.Id);
			Field(x => x.Title);
			Field(x => x.TreeTitle);
			Field<IntGraphType>("trackNumber", resolve: context => context.Source.TrackNumber);
			Field<NonNullGraphType<TimeSpanSecondsGraphType>>("duration");
			Field<NonNullGraphType<DiscType>>("disc", resolve: context => context.Source.Disc);
			Field<ArtistType>("artist", resolve: context => context.Source.Artist);
			Field<GenreType>("genre", resolve: context => context.Source.Genre);
			Field<RatingEnumType>("rating");
			Field(x => x.BitRate, true);
			Field(x => x.LastPlaybackTime, true);
			Field(x => x.PlaybacksCount);
			Field(x => x.DeleteDate, true);
			Field(x => x.DeleteComment, true);
		}
	}
}
