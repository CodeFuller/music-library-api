using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class GraphQLResponse
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		[DataMember(Name = "data")]
		public JToken? Data { get; set; }

		[DataMember(Name = "errors")]
		public IReadOnlyCollection<GraphQLError>? Errors { get; set; }
	}
}
