using Newtonsoft.Json.Linq;

namespace MusicLibraryApi.Internal
{
	public class GraphQLQuery
	{
		// CF TEMP: Do we need this?
		public string OperationName { get; set; }

		// CF TEMP: Do we need this?
		public string NamedQuery { get; set; }

		public string Query { get; set; }

		public JObject Variables { get; set; }
	}
}
