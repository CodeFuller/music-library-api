using System.IO;

namespace MusicLibraryApi.Client.Internal
{
	public class FileUpload
	{
		public Stream Stream { get; }

		public string FileName { get; }

		public FileUpload(Stream stream, string fileName)
		{
			Stream = stream;
			FileName = fileName;
		}
	}
}
