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
			Field(x => x.Genre.Name).Name("genre");
			Field<IntGraphType>("rating", resolve: context => (int?)context.Source.Rating);
			Field(x => x.BitRate, true);
			Field(x => x.FileSize);
			Field(x => x.Checksum);
			Field(x => x.Artist.Name).Name("artist");
			Field(x => x.LastPlaybackTime, true);
			Field(x => x.PlaybacksCount);
		}
	}
}
