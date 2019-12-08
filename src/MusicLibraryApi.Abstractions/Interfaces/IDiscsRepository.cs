using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IDiscsRepository
	{
		Task<int> CreateDisc(int folderId, Disc disc, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken);

		Task<Disc> GetDisc(int discId, CancellationToken cancellationToken);
	}
}
