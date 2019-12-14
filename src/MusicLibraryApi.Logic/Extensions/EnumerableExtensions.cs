using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicLibraryApi.Logic.Extensions
{
	public static class EnumerableExtensions
	{
		public static TimeSpan Sum(this IEnumerable<TimeSpan> source)
		{
			return source.Aggregate(TimeSpan.Zero, (sum, duration) => sum + duration);
		}
	}
}
