using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public abstract class BasicDataComparer<TData> : IComparer
		where TData : class
	{
		protected abstract IEnumerable<Func<TData, TData, int>> PropertyComparers { get; }

		public int Compare(object? x, object? y)
		{
			// Using unsafe type cast to catch objects of incorrect type. Otherwise Compare() will return 0 and asserts will always pass.
			var item1 = (TData?)x;
			var item2 = (TData?)y;

			if (Object.ReferenceEquals(item1, null) && Object.ReferenceEquals(item2, null))
			{
				return 0;
			}

			if (Object.ReferenceEquals(item1, null))
			{
				return -1;
			}

			if (Object.ReferenceEquals(item2, null))
			{
				return 1;
			}

			foreach (var propertyComparer in PropertyComparers)
			{
				var cmp = propertyComparer(item1, item2);
				if (cmp != 0)
				{
					return cmp;
				}
			}

			return 0;
		}

		protected Func<TData, TData, int> FieldComparer<T>(Expression<Func<TData, T?>> expression)
			where T : struct
		{
			var f = expression.Compile();
			return (x, y) => Compare(f(x), f(y));
		}

		protected Func<TData, TData, int> FieldComparer<T>(Expression<Func<TData, T?>> expression, IComparer comparer)
			where T : class
		{
			var f = expression.Compile();
			return (x, y) => comparer.Compare(f(x), f(y));
		}

		protected Func<TData, TData, int> FieldComparer(Expression<Func<TData, string?>> expression)
		{
			var f = expression.Compile();
			return (x, y) => Compare(f(x), f(y));
		}

		protected Func<TData, TData, int> FieldComparer<TItem>(Expression<Func<TData, IReadOnlyCollection<TItem>?>> expression, IComparer<IReadOnlyCollection<TItem>?> itemsComparer)
		{
			var f = expression.Compile();
			return (x, y) => itemsComparer.Compare(f(x), f(y));
		}

		protected int Compare(string? s1, string? s2)
		{
			return String.Compare(s1, s2, StringComparison.Ordinal);
		}

		protected int Compare<T>(T? x, T? y)
			 where T : struct
		{
			return Nullable.Compare(x, y);
		}
	}
}
