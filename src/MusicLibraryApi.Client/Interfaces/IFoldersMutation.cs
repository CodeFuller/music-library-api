using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Folders;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IFoldersMutation
	{
		Task<int> CreateFolder(InputFolderData folderData, CancellationToken cancellationToken);
	}
}
