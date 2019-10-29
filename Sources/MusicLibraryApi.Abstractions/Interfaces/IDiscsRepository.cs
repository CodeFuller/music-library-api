using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IDiscsRepository
	{
		Task<string> CreateDisc(Disc disc, CancellationToken cancellationToken);

		Task<ICollection<Disc>> GetDiscs(CancellationToken cancellationToken);

		Task<Disc> GetDisc(string discId, CancellationToken cancellationToken);

		Task UpdateDisc(Disc disc, CancellationToken cancellationToken);

		Task DeleteDisc(string discId, CancellationToken cancellationToken);
	}
}
