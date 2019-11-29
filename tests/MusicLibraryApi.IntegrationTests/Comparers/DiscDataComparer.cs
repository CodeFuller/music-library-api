using System;
using System.Collections;
using MusicLibraryApi.Client.Contracts.Discs;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class DiscDataComparer : IComparer
	{
		public int Compare(object? x, object? y)
		{
			// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
			var d1 = (OutputDiscData?)x;
			var d2 = (OutputDiscData?)y;

			if (Object.ReferenceEquals(d1, null) && Object.ReferenceEquals(d2, null))
			{
				return 0;
			}

			if (Object.ReferenceEquals(d1, null))
			{
				return -1;
			}

			if (Object.ReferenceEquals(d2, null))
			{
				return 1;
			}

			var cmp = Nullable.Compare(d1.Id, d2.Id);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = Nullable.Compare(d1.Year, d2.Year);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = String.Compare(d1.Title, d2.Title, StringComparison.Ordinal);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = String.Compare(d1.AlbumTitle, d2.AlbumTitle, StringComparison.Ordinal);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = Nullable.Compare(d1.AlbumOrder, d2.AlbumOrder);
			if (cmp != 0)
			{
				return cmp;
			}

			cmp = Nullable.Compare(d1.DeleteDate, d2.DeleteDate);
			if (cmp != 0)
			{
				return cmp;
			}

			return String.Compare(d1.DeleteComment, d2.DeleteComment, StringComparison.Ordinal);
		}
	}
}
