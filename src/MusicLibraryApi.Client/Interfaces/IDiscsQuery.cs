using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Output;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IDiscsQuery
	{
		IAsyncEnumerable<DiscDto> GetDiscs(QueryFieldSet fields, CancellationToken cancellationToken);

		Task<DiscDto> GetDisc(int discId, QueryFieldSet fields, CancellationToken cancellationToken);
	}
}
