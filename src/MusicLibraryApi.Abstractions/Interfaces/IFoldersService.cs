using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IFoldersService
	{
		Task<int> CreateFolder(int? parentFolderId, string folderName, CancellationToken cancellationToken);

		Task<Folder> GetFolder(int? folderId, bool loadSubfolders, bool loadDiscs, CancellationToken cancellationToken);
	}
}
