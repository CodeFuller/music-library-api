using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryApi.Logic.Interfaces
{
	internal interface IContentStorage
	{
		Task CreateFolder(string path, CancellationToken cancellationToken);

		Task DeleteEmptyFolder(string path, CancellationToken cancellationToken);

		Task StoreContent(string path, byte[] content, CancellationToken cancellationToken);
	}
}
