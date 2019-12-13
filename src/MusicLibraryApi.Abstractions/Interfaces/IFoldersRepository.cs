using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IFoldersRepository
	{
		Task<int> GetRooFolderId(CancellationToken cancellationToken);

		Task AddFolder(Folder folder, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Folder>> GetFolders(IEnumerable<int> folderIds, CancellationToken cancellationToken);

		Task<Folder> GetFolder(int folderId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Folder>> GetSubfoldersByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken);
	}
}
