using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IFoldersRepository
	{
		Task<int> GetRooFolderId(CancellationToken cancellationToken);

		Task<int> CreateFolder(int parentFolderId, Folder folder, CancellationToken cancellationToken);

		Task<Folder> GetFolder(int folderId, bool loadSubfolders, bool loadDiscs, CancellationToken cancellationToken);
	}
}
