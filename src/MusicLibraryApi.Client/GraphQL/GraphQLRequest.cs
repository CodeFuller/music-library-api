using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
	public class GraphQLRequest
	{
		[DataMember(Name = "query")]
		public string? Query { get; set; }

		[DataMember(Name = "variables")]
		public object? Variables { get; set; }
	}
}
