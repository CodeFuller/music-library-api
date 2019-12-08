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

			// TBD: Return complex genre type
			Field("genre", x => x.Genre != null ? x.Genre.Name : null);
			Field<RatingEnumType>("rating");
			Field(x => x.BitRate, true);

			// TBD: Return complex artist type
			Field("artist", x => x.Artist != null ? x.Artist.Name : null);
			Field(x => x.LastPlaybackTime, true);
			Field(x => x.PlaybacksCount);
			Field(x => x.DeleteDate, true);
			Field(x => x.DeleteComment, true);
		}
	}
}
