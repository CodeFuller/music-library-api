using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class GenreNotFoundException : NotFoundException
	{
		public GenreNotFoundException()
		{
		}

		public GenreNotFoundException(string message)
			: base(message)
		{
		}

		public GenreNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected GenreNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
