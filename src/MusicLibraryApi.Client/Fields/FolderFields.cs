using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class FolderFields
	{
		public static QueryField<FolderQuery> Id { get; } = new QueryField<FolderQuery>("id");

		public static QueryField<FolderQuery> Name { get; } = new QueryField<FolderQuery>("name");

		public static QueryField<FolderQuery> Subfolders { get; } = new QueryField<FolderQuery>("subfolders");

		public static QueryField<FolderQuery> Discs { get; } = new QueryField<FolderQuery>("discs");

		public static QueryFieldSet<FolderQuery> All { get; } = Id + Name + Subfolders + Discs;
	}
}
