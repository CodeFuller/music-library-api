using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IFoldersService
	{
		Task<int> CreateFolder(int? parentFolderId, string folderName, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Folder>> GetSubfolders(int? folderId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetFolderDiscs(int? folderId, CancellationToken cancellationToken);
	}
}
