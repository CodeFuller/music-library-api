using System;
using System.Linq;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Fields
{
	public class ComplexQueryField<TParentQuery, TNestedQuery> : QueryField<TParentQuery>
	{
		private readonly QueryFieldSet<TNestedQuery> nestedFields;

		public override string QuerySelection => Invariant($"{Name} {{ {nestedFields.QuerySelection} }}");

		public ComplexQueryField(string name, QueryFieldSet<TNestedQuery> nestedFields)
			: base(name)
		{
			if (!nestedFields.Any())
			{
				throw new InvalidOperationException($"The sub-selection on {name} field is empty");
			}

			this.nestedFields = nestedFields;
		}
	}
}
