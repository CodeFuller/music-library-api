using System;
using System.Collections.Generic;
using System.Linq;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Fields
{
	public class ComplexQueryField<TParentQuery, TNestedQuery> : QueryField<TParentQuery>
	{
		private readonly QueryFieldSet<TNestedQuery> nestedFields;

		private readonly IReadOnlyCollection<QueryVariable> variables;

		public override string QuerySelection => Invariant($"{Name}{BuildVariableArguments()} {{ {nestedFields.QuerySelection} }}");

		public override string VariablesDefinition => BuildVariablesDefinition();

		public ComplexQueryField(string name, QueryFieldSet<TNestedQuery> nestedFields, params QueryVariable[] variables)
			: base(name)
		{
			if (!nestedFields.Any())
			{
				throw new InvalidOperationException($"The sub-selection on {name} field is empty");
			}

			this.nestedFields = nestedFields;

			this.variables = variables ?? Array.Empty<QueryVariable>();
		}

		private string BuildVariableArguments()
		{
			if (!variables.Any())
			{
				return String.Empty;
			}

			return Invariant($"({String.Join(", ", variables.Select(v => v.ArgumentDeclaration))})");
		}

		private string BuildVariablesDefinition()
		{
			if (!variables.Any())
			{
				return String.Empty;
			}

			return String.Join(", ", variables.Select(v => v.VariableDefinition));
		}
	}
}
