using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IDiscsRepository
	{
		Task AddDisc(Disc disc, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken);

		Task<Disc> GetDisc(int discId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken);
	}
}
