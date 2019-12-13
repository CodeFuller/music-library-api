namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class AddPlaybackResult
	{
		public int NewPlaybackId { get; }

		public AddPlaybackResult(int newPlaybackId)
		{
			NewPlaybackId = newPlaybackId;
		}
	}
}
