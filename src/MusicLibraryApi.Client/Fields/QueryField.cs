using System;

namespace MusicLibraryApi.Client.Fields
{
	public class QueryField<TQuery>
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

		public bool Equals(QueryField<TQuery>? f)
		{
			if (Object.ReferenceEquals(f, null))
			{
				return false;
			}

			if (Object.ReferenceEquals(this, f))
			{
				return true;
			}

			return String.Equals(Name, f.Name, StringComparison.Ordinal);
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as QueryField<TQuery>);
		}

		public override int GetHashCode()
		{
			return Name?.GetHashCode() ?? 0;
		}

		public static bool operator ==(QueryField<TQuery>? f1, QueryField<TQuery>? f2)
		{
			if (Object.ReferenceEquals(f1, null))
			{
				return Object.ReferenceEquals(f2, null);
			}

			return f1.Equals(f2);
		}

		public static bool operator !=(QueryField<TQuery>? f1, QueryField<TQuery>? f2)
		{
			return !(f1 == f2);
		}
	}
}
