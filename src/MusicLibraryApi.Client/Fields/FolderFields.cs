using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;

namespace MusicLibraryApi.Client.Fields
{
	public static class FolderFields
	{
		public static QueryField<OutputFolderData> Id { get; } = new QueryField<OutputFolderData>("id");

		public static QueryField<OutputFolderData> Name { get; } = new QueryField<OutputFolderData>("name");

		public static ComplexQueryField<OutputFolderData, OutputFolderData> Subfolders(QueryFieldSet<OutputFolderData> subfolderFields)
		{
			return new ComplexQueryField<OutputFolderData, OutputFolderData>("subfolders", subfolderFields);
		}

		public static ComplexQueryField<OutputFolderData, OutputDiscData> Discs(QueryFieldSet<OutputDiscData> discFields)
		{
			return new ComplexQueryField<OutputFolderData, OutputDiscData>("discs", discFields, new QueryVariable("Boolean", "includeDeletedDiscs"));
		}

		public static QueryFieldSet<OutputFolderData> All { get; } = Id + Name;
	}
}
