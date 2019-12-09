using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Fields;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IFoldersQuery
	{
		Task<OutputFolderData> GetFolder(int? folderId, QueryFieldSet<OutputFolderData> fields, CancellationToken cancellationToken, bool includeDeletedDiscs = false);
	}
}
