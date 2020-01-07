using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Logic.Interfaces
{
	public interface IStorageService
	{
		Task<string> CreateFolder(Folder folder, CancellationToken cancellationToken);

		Task StoreSong(Song song, Stream contentStream, CancellationToken cancellationToken);
	}
}
