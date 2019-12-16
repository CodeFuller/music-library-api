using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.GraphQL
{
	[DataContract]
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class GraphQLError
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		[DataMember]
		public string? Message { get; set; }
	}
}
