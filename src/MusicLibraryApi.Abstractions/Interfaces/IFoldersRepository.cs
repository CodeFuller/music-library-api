using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IFoldersRepository
	{
		Task<int> CreateFolder(int? parentFolderId, Folder folder, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Folder>> GetSubfolders(int? folderId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetFolderDiscs(int? folderId, CancellationToken cancellationToken);
	}
}
