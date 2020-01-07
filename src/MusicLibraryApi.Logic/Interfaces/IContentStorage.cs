using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryApi.Logic.Interfaces
{
	internal interface IContentStorage
	{
		Task CreateFolder(string folderPath, CancellationToken cancellationToken);

		Task StoreContent(string contentPath, byte[] content, CancellationToken cancellationToken);
	}
}
