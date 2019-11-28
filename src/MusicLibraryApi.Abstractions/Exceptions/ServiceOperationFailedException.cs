using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class ServiceOperationFailedException : Exception
	{
		public ServiceOperationFailedException()
		{
		}

		public ServiceOperationFailedException(string message)
			: base(message)
		{
		}

		public ServiceOperationFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ServiceOperationFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
