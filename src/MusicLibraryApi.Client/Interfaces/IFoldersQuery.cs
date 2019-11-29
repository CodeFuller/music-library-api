using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IFoldersQuery
	{
		Task<IReadOnlyCollection<OutputFolderData>> GetSubfolders(int? folderId, QueryFieldSet<FolderQuery> fields, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<OutputDiscData>> GetFolderDiscs(int? folderId, QueryFieldSet<DiscQuery> fields, CancellationToken cancellationToken);
	}
}
