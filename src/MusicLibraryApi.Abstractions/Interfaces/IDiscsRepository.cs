using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IDiscsRepository
	{
		Task<int> AddDisc(int folderId, Disc disc, CancellationToken cancellationToken);

		Task<IEnumerable<Disc>> GetAllDiscs(CancellationToken cancellationToken);

		Task<Disc> GetDisc(int discId, CancellationToken cancellationToken);
	}
}
