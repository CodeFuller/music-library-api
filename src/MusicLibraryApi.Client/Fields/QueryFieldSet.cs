using System.Collections;
using System.Collections.Generic;

namespace MusicLibraryApi.Client.Fields
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
	public class QueryFieldSet : IEnumerable<QueryField>
#pragma warning restore CA1710 // Identifiers should have correct suffix
	{
		private readonly IReadOnlyCollection<QueryField> fields;

		public QueryFieldSet(QueryField field)
		{
			fields = new List<QueryField> { field };
		}

		public QueryFieldSet(QueryField f1, QueryField f2)
		{
			fields = new List<QueryField> { f1, f2 };
		}

		public QueryFieldSet(QueryFieldSet set, QueryField field)
		{
			fields = new List<QueryField>(set.fields) { field };
		}

		public static implicit operator QueryFieldSet(QueryField field)
		{
			return new QueryFieldSet(field);
		}

		public static QueryFieldSet FromQueryField(QueryField field)
		{
			return new QueryFieldSet(field);
		}

		public static QueryFieldSet operator +(QueryFieldSet set, QueryField field)
		{
			return new QueryFieldSet(set, field);
		}

		public static QueryFieldSet Add(QueryFieldSet set, QueryField field)
		{
			return set + field;
		}

		public IEnumerator<QueryField> GetEnumerator()
		{
			return fields.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
