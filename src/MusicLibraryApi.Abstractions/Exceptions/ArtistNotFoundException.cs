using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class ArtistNotFoundException : NotFoundException
	{
		public ArtistNotFoundException()
		{
		}

		public ArtistNotFoundException(string message)
			: base(message)
		{
		}

		public ArtistNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ArtistNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
