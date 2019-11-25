namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateArtistResult
	{
		public int NewArtistId { get; }

		public CreateArtistResult(int newArtistId)
		{
			NewArtistId = newArtistId;
		}
	}
}
