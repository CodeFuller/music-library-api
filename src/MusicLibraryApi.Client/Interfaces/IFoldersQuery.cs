using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IFoldersQuery
	{
		Task<OutputFolderData> GetFolder(int? folderId, QueryFieldSet<FolderQuery> fields, CancellationToken cancellationToken);
	}
}
