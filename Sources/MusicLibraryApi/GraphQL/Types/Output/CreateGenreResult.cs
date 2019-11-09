namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateGenreResult
	{
		public int NewGenreId { get; }

		public CreateGenreResult(int newGenreId)
		{
			NewGenreId = newGenreId;
		}
	}
}
