using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class PlaybackNotFoundException : NotFoundException
	{
		public PlaybackNotFoundException()
		{
		}

		public PlaybackNotFoundException(string message)
			: base(message)
		{
		}

		public PlaybackNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected PlaybackNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
