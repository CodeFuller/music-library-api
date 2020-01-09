using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryApi.Logic.Interfaces
{
	internal interface IContentStorage
	{
		Task CreateFolder(IEnumerable<string> pathParts, CancellationToken cancellationToken);

		Task DeleteEmptyFolder(IEnumerable<string> pathParts, CancellationToken cancellationToken);

		Task StoreContent(IEnumerable<string> pathParts, byte[] content, CancellationToken cancellationToken);

		Task DeleteContent(IEnumerable<string> pathParts, CancellationToken cancellationToken);
	}
}
