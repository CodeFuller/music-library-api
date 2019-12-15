using System;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class FolderInput
	{
		public string? Name { get; set; }

		public int? ParentFolderId { get; set; }

		public Folder ToModel()
		{
			if (ParentFolderId == null)
			{
				throw new InvalidOperationException("Parent folder id is not set");
			}

			if (String.IsNullOrEmpty(Name))
			{
				throw new InvalidOperationException("The folder name is not set");
			}

			return new Folder
			{
				Name = Name,
				ParentFolderId = ParentFolderId,
			};
		}
	}
}
