using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class CollectionsComparer<T> : IComparer<IReadOnlyCollection<T>?>
	{
		private readonly IComparer itemsComparer;

		public CollectionsComparer(IComparer itemsComparer)
		{
			this.itemsComparer = itemsComparer ?? throw new ArgumentNullException(nameof(itemsComparer));
		}

		public int Compare(IReadOnlyCollection<T>? c1, IReadOnlyCollection<T>? c2)
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

			var cmp = c1.Count.CompareTo(c2.Count);
			if (cmp != 0)
			{
				return cmp;
			}

			foreach (var (first, second) in c1.Zip(c2))
			{
				cmp = itemsComparer.Compare(first, second);
				if (cmp != 0)
				{
					return cmp;
				}
			}

			return 0;
		}
	}
}
