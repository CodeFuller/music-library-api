using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class DiscNotFoundException : NotFoundException
	{
		public DiscNotFoundException()
		{
		}

		public DiscNotFoundException(string message)
			: base(message)
		{
		}

		public DiscNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected DiscNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
