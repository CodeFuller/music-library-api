namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class CreateFolderResult
	{
		public int NewFolderId { get; }

		public CreateFolderResult(int newFolderId)
		{
			NewFolderId = newFolderId;
		}
	}
}
