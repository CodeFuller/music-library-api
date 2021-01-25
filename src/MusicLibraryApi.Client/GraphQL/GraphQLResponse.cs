using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
	internal class GraphQLResponse
	{
		[DataMember(Name = "data")]
		public JToken? Data { get; set; }

		[DataMember(Name = "errors")]
		public IReadOnlyCollection<GraphQLError>? Errors { get; set; }
	}
}
