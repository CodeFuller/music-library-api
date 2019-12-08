namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateSongResult
	{
		public int NewSongId { get; }

		public CreateSongResult(int newSongId)
		{
			NewSongId = newSongId;
		}
	}
}
