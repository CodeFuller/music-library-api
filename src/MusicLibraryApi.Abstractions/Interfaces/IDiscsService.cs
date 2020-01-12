using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IDiscsService
	{
		Task<int> CreateDisc(Disc disc, CancellationToken cancellationToken);

		Task<int> CreateDisc(Disc disc, Stream coverContent, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken);

		Task<IDictionary<int, Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken);

		Task<Disc> GetDisc(int discId, CancellationToken cancellationToken);

		Task<ILookup<int, Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, bool includeDeletedDiscs, CancellationToken cancellationToken);
	}
}
