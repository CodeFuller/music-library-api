using System;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class PlaybackInput
	{
		public DateTimeOffset? PlaybackTime { get; set; }
	}
}
