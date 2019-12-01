using System;

namespace MusicLibraryApi.Client.Fields
{
	public class QueryField<TQuery>
	{
		public string Name { get; }

		public virtual string QuerySelection => Name;

		public QueryField(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new InvalidOperationException($"The field name is invalid: {name}");
			}

			Name = name;
		}

		public static QueryFieldSet<TQuery> operator +(QueryField<TQuery> f1, QueryField<TQuery> f2)
		{
			return new QueryFieldSet<TQuery>(f1, f2);
		}

#pragma warning disable CA1000 // Do not declare static members on generic types
		public static QueryFieldSet<TQuery> Add(QueryField<TQuery> f1, QueryField<TQuery> f2)
#pragma warning restore CA1000 // Do not declare static members on generic types
		{
			return f1 + f2;
		}
	}
}
