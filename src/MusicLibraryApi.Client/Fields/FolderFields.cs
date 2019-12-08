using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class FolderFields
	{
		public static QueryField<FolderQuery> Id { get; } = new QueryField<FolderQuery>("id");

		public static QueryField<FolderQuery> Name { get; } = new QueryField<FolderQuery>("name");

		public static ComplexQueryField<FolderQuery, FolderQuery> Subfolders(QueryFieldSet<FolderQuery> subfolderFields)
		{
			return new ComplexQueryField<FolderQuery, FolderQuery>("subfolders", subfolderFields);
		}

		public static ComplexQueryField<FolderQuery, DiscQuery> Discs(QueryFieldSet<DiscQuery> discFields)
		{
			return new ComplexQueryField<FolderQuery, DiscQuery>("discs", discFields);
		}

		public static QueryFieldSet<FolderQuery> All { get; } = Id + Name;
	}
}
