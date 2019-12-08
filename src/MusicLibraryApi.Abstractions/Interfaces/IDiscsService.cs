using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IDiscsService
	{
		Task<int> CreateDisc(int folderId, Disc disc, CancellationToken cancellationToken);

		Task<Disc> GetDisc(int discId, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken);
	}
}
