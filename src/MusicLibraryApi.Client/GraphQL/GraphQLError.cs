using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
	public class GraphQLError
	{
		[DataMember]
		public string? Message { get; set; }
	}
}
