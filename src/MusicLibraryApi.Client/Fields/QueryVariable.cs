using System;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Fields
{
	public class QueryVariable
	{
		public string Type { get; }

		public string Name { get; }

		public string ArgumentDeclaration => Invariant($"{Name}: ${Name}");

		public string VariableDefinition => Invariant($"${Name}: {Type}");

		public QueryVariable(string type, string name)
		{
			Type = type ?? throw new ArgumentNullException(nameof(type));
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}
	}
}
