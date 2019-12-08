using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public abstract class NotFoundException : Exception
	{
		protected NotFoundException()
		{
		}

		protected NotFoundException(string message)
			: base(message)
		{
		}

		protected NotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected NotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
