using Newtonsoft.Json.Linq;

namespace MusicLibraryApi.Internal
{
	public class GraphQLQuery
	{
		public string OperationName { get; set; }

		public string NamedQuery { get; set; }

		public string Query { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only - The setter is called by the model binder.
		public JObject Variables { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
	}
}
