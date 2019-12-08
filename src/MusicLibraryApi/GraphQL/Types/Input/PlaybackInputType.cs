using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class PlaybackInputType : InputObjectGraphType<PlaybackInput>
	{
		public PlaybackInputType()
		{
			Name = "PlaybackInput";

			Field<NonNullGraphType<DateTimeOffsetGraphType>>("playbackTime");
		}
	}
}
