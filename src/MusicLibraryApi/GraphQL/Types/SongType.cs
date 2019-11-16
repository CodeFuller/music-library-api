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
			Field<IntGraphType>("trackNumber", resolve: context => context.Source.TrackNumber);
			Field(x => x.Duration, true);
			Field("genre", x => x.Genre != null ? x.Genre.Name : null);
			Field<RatingEnumType>("rating");
			Field(x => x.BitRate, true);
			Field(x => x.FileSize);
			Field(x => x.Checksum);
			Field("artist", x => x.Artist != null ? x.Artist.Name : null);
			Field(x => x.LastPlaybackTime, true);
			Field(x => x.PlaybacksCount);
		}
	}
}
