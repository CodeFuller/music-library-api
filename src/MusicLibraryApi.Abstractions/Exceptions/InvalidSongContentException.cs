using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class InvalidSongContentException : Exception
	{
		public InvalidSongContentException()
		{
		}

		public InvalidSongContentException(string message)
			: base(message)
		{
		}

		public InvalidSongContentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected InvalidSongContentException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
