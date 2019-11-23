using System;

namespace MusicLibraryApi.Client
{
	public class QueryField
	{
		public string Name { get; }

		public QueryField(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new InvalidOperationException($"The field name is invalid: {name}");
			}

			Name = name;
		}

		public static QueryFieldSet operator +(QueryField f1, QueryField f2)
		{
			return new QueryFieldSet(f1, f2);
		}

		public static QueryFieldSet Add(QueryField f1, QueryField f2)
		{
			return f1 + f2;
		}
	}
}
