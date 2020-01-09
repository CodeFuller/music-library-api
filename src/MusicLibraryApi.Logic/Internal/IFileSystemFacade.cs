using System.IO;

namespace MusicLibraryApi.Logic.Internal
{
	internal interface IFileSystemFacade
	{
		bool DirectoryExists(string path);

		void CreateDirectory(string path);

		void DeleteDirectory(string path);

		Stream OpenFile(string path, FileMode mode);

		void DeleteFile(string path);

		void ClearReadOnlyAttribute(string fileName);

		void SetReadOnlyAttribute(string fileName);
	}
}
