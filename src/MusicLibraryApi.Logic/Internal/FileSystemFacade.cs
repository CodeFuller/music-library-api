using System.IO;

namespace MusicLibraryApi.Logic.Internal
{
	internal class FileSystemFacade : IFileSystemFacade
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
