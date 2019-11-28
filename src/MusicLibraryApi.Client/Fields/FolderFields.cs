﻿using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class FolderFields
	{
		public static QueryField<FolderQuery> Id { get; } = new QueryField<FolderQuery>("id");

		public static QueryField<FolderQuery> Name { get; } = new QueryField<FolderQuery>("name");

		public static QueryField<FolderQuery> ParentFolderId { get; } = new QueryField<FolderQuery>("parentFolderId");

		public static QueryFieldSet<FolderQuery> All { get; } = Id + Name + ParentFolderId;
	}
}