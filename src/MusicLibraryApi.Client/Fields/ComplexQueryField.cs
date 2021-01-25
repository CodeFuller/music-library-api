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

		internal override string QuerySelection => Invariant($"{Name}{BuildVariableArguments()} {{ {nestedFields.QuerySelection} }}");

		internal override string VariablesDefinition => BuildVariablesDefinition();

		internal ComplexQueryField(string name, QueryFieldSet<TNestedQuery> nestedFields, params QueryVariable[] variables)
			: base(name)
		{
			if (!nestedFields.Any())
			{
				throw new InvalidOperationException($"The sub-selection on {name} field is empty");
			}

			this.nestedFields = nestedFields;

#pragma warning disable CA1508 // Avoid dead conditional code
			this.variables = variables ?? Array.Empty<QueryVariable>();
#pragma warning restore CA1508 // Avoid dead conditional code
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
