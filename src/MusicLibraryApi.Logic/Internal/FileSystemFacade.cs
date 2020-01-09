using System.IO;

namespace MusicLibraryApi.Logic.Internal
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class FileSystemFacade : IFileSystemFacade
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public void DeleteDirectory(string path)
		{
			Directory.Delete(path);
		}

		public Stream OpenFile(string path, FileMode mode)
		{
			return File.Open(path, mode);
		}

		public void DeleteFile(string path)
		{
			File.Delete(path);
		}

		public void ClearReadOnlyAttribute(string fileName)
		{
			_ = new FileInfo(fileName)
			{
				IsReadOnly = false,
			};
		}

		public void SetReadOnlyAttribute(string fileName)
		{
			_ = new FileInfo(fileName)
			{
				IsReadOnly = true,
			};
		}
	}
}
