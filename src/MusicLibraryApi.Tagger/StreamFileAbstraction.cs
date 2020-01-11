using System;
using System.IO;

namespace MusicLibraryApi.Tagger
{
	internal class StreamFileAbstraction : TagLib.File.IFileAbstraction
	{
		private readonly Stream contentStream;

		public string Name { get; }

		public Stream ReadStream => contentStream;

		public Stream WriteStream => contentStream;

		public StreamFileAbstraction(Stream stream, string name)
		{
			this.contentStream = stream ?? throw new ArgumentNullException(nameof(stream));
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public void CloseStream(Stream stream)
		{
			// We do not close the stream here.
			// If the stream is closed, then implementations of ReadStream and WriteStream should re-open it.
		}
	}
}
