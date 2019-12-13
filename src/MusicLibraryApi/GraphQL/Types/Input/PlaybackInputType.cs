using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class PlaybackInputType : InputObjectGraphType<PlaybackInput>
	{
		public PlaybackInputType()
		{
			Name = "PlaybackInput";

			Field<NonNullGraphType<IdGraphType>>("songId");
			Field<NonNullGraphType<DateTimeOffsetGraphType>>("playbackTime");
		}
	}
}
