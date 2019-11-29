using System;
using System.Collections;
using MusicLibraryApi.Client.Contracts.Folders;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class FolderDataComparer : IComparer
	{
		public int Compare(object? x, object? y)
		{
			// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
			var f1 = (OutputFolderData?)x;
			var f2 = (OutputFolderData?)y;

			if (Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
			{
				return 0;
			}

			if (Object.ReferenceEquals(f1, null))
			{
				return -1;
			}

			if (Object.ReferenceEquals(f2, null))
			{
				return 1;
			}

			var cmp = Nullable.Compare(f1.Id, f2.Id);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = String.Compare(f1.Name, f2.Name, StringComparison.Ordinal);
			if (cmp != 0)
			{
				return cmp;
			}

			return Nullable.Compare(f1.ParentFolderId, f2.ParentFolderId);
		}
	}
}
