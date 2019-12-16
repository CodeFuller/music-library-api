using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
	internal class GraphQLRequest
	{
		[DataMember(Name = "query")]
		public string? Query { get; set; }

		[DataMember(Name = "variables")]
		public object? Variables { get; set; }
	}
}
