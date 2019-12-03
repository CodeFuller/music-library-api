using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MusicLibraryApi.Client.Contracts.Folders;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class FolderDataComparer : IComparer
	{
		private readonly DiscDataComparer discsComparer = new DiscDataComparer();

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

			cmp = CompareCollections(f1.Subfolders, f2.Subfolders, this);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = CompareCollections(f1.Discs, f2.Discs, discsComparer);
			if (cmp != 0)
			{
				return cmp;
			}

			return 0;
		}

		private int CompareCollections<T>(IReadOnlyCollection<T>? c1, IReadOnlyCollection<T>? c2, IComparer comparer)
		{
			if (Object.ReferenceEquals(c1, null) && Object.ReferenceEquals(c2, null))
			{
				return 0;
			}

			if (Object.ReferenceEquals(c1, null))
			{
				return -1;
			}

			if (Object.ReferenceEquals(c2, null))
			{
				return 1;
			}

			if (c1.Count != c2.Count)
			{
				return c1.Count < c2.Count ? -1 : 1;
			}

			foreach (var pair in c1.Zip(c2))
			{
				var cmp = comparer.Compare(pair.First, pair.Second);
				if (cmp != 0)
				{
					return cmp;
				}
			}

			return 0;
		}
	}
}
