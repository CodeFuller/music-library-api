using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class SongNotFoundException : NotFoundException
	{
		public SongNotFoundException()
		{
		}

		public SongNotFoundException(string message)
			: base(message)
		{
		}

		public SongNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected SongNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
