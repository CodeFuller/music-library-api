using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Exceptions
{
	[Serializable]
	public class GraphQLRequestFailedException : Exception
	{
		public GraphQLRequestFailedException()
		{
		}

		public GraphQLRequestFailedException(string message)
			: base(message)
		{
		}

		public GraphQLRequestFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected GraphQLRequestFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
