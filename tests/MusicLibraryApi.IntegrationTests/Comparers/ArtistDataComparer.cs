using System;
using System.Collections;
using MusicLibraryApi.Client.Contracts.Artists;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class ArtistDataComparer : IComparer
	{
		public int Compare(object? x, object? y)
		{
			// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
			var a1 = (OutputArtistData?)x;
			var a2 = (OutputArtistData?)y;

			if (Object.ReferenceEquals(a1, null) && Object.ReferenceEquals(a2, null))
			{
				return 0;
			}

			if (Object.ReferenceEquals(a1, null))
			{
				return -1;
			}

			if (Object.ReferenceEquals(a2, null))
			{
				return 1;
			}

			var cmp = Nullable.Compare(a1.Id, a2.Id);
			if (cmp != 0)
			{
				return cmp;
			}

			return String.Compare(a1.Name, a2.Name, StringComparison.Ordinal);
		}
	}
}
