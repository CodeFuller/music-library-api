using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Logic.Interfaces
{
	public interface IStorageService
	{
		Task CreateFolder(Folder folder, CancellationToken cancellationToken);

		Task CreateDisc(Disc disc, CancellationToken cancellationToken);

		Task StoreDiscCover(Disc disc, Stream coverContentStream, CancellationToken cancellationToken);

		Task StoreSong(Song song, Stream contentStream, CancellationToken cancellationToken);

		Task RollbackFolderCreation(Folder folder, CancellationToken cancellationToken);

		Task RollbackDiscCreation(Disc disc, CancellationToken cancellationToken);

		Task RollbackSongCreation(Song song, CancellationToken cancellationToken);
	}
}
