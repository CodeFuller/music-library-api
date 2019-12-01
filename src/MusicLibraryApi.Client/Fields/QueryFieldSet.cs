using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MusicLibraryApi.Client.Fields
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
	public class QueryFieldSet<TQuery> : IEnumerable<QueryField<TQuery>>
#pragma warning restore CA1710 // Identifiers should have correct suffix
	{
		private readonly IReadOnlyCollection<QueryField<TQuery>> fields;

		public string QuerySelection => String.Join(" ", fields.Select(f => f.QuerySelection));

		public QueryFieldSet(QueryField<TQuery> field)
		{
			fields = new List<QueryField<TQuery>> { field };
		}

		public QueryFieldSet(QueryField<TQuery> f1, QueryField<TQuery> f2)
		{
			fields = new List<QueryField<TQuery>> { f1, f2 };
		}

		public QueryFieldSet(QueryFieldSet<TQuery> set, QueryField<TQuery> field)
		{
			fields = new List<QueryField<TQuery>>(set.fields) { field };
		}

		public static implicit operator QueryFieldSet<TQuery>(QueryField<TQuery> field)
		{
			return new QueryFieldSet<TQuery>(field);
		}

#pragma warning disable CA1000 // Do not declare static members on generic types
		public static QueryFieldSet<TQuery> FromQueryField(QueryField<TQuery> field)
#pragma warning restore CA1000 // Do not declare static members on generic types
		{
			return new QueryFieldSet<TQuery>(field);
		}

		public static QueryFieldSet<TQuery> operator +(QueryFieldSet<TQuery> set, QueryField<TQuery> field)
		{
			return new QueryFieldSet<TQuery>(set, field);
		}

#pragma warning disable CA1000 // Do not declare static members on generic types
		public static QueryFieldSet<TQuery> Add(QueryFieldSet<TQuery> set, QueryField<TQuery> field)
#pragma warning restore CA1000 // Do not declare static members on generic types
		{
			return set + field;
		}

		public IEnumerator<QueryField<TQuery>> GetEnumerator()
		{
			return fields.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
