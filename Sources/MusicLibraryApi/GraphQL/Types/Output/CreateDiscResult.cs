namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateDiscResult
	{
		public int NewDiscId { get; }

		public CreateDiscResult(int newDiscId)
		{
			NewDiscId = newDiscId;
		}
	}
}
