using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
	internal class GraphQLError
	{
		[DataMember]
		public string? Message { get; set; }
	}
}
