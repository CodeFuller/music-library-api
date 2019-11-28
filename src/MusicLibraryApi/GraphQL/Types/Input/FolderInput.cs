using System;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class FolderInput
	{
		public string? Name { get; set; }

		public int? ParentFolderId { get; set; }

		public string GetFolderName()
		{
			if (String.IsNullOrEmpty(Name))
			{
				throw new InvalidOperationException("The folder name is not set");
			}

			return Name;
		}
	}
}
