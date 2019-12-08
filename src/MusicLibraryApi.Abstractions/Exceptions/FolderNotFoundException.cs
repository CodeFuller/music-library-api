using System;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Abstractions.Exceptions
{
	[Serializable]
	public class FolderNotFoundException : NotFoundException
	{
		public FolderNotFoundException()
		{
		}

		public FolderNotFoundException(string message)
			: base(message)
		{
		}

		public FolderNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected FolderNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
