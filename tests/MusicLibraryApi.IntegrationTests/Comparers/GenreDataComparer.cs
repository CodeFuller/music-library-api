using System;
using System.Collections;
using MusicLibraryApi.Client.Contracts.Genres;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class GenreDataComparer : IComparer
	{
		public int Compare(object? x, object? y)
		{
			// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
			var g1 = (OutputGenreData?)x;
			var g2 = (OutputGenreData?)y;

			if (Object.ReferenceEquals(g1, null) && Object.ReferenceEquals(g2, null))
			{
				return 0;
			}

			if (Object.ReferenceEquals(g1, null))
			{
				return -1;
			}

			if (Object.ReferenceEquals(g2, null))
			{
				return 1;
			}

			var cmp = Nullable.Compare(g1.Id, g2.Id);
			if (cmp != 0)
			{
				return cmp;
			}

			return String.Compare(g1.Name, g2.Name, StringComparison.Ordinal);
		}
	}
}
