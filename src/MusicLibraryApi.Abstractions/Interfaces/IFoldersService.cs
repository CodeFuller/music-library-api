using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IFoldersService
	{
		Task<int> CreateFolder(int parentFolderId, string folderName, CancellationToken cancellationToken);

		Task<IDictionary<int, Folder>> GetFolders(IEnumerable<int> folderIds, CancellationToken cancellationToken);

		Task<Folder> GetFolder(int? folderId, CancellationToken cancellationToken);

		Task<ILookup<int, Folder>> GetSubfoldersByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken);
	}
}
