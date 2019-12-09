using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Fields;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IDiscsQuery
	{
		Task<IReadOnlyCollection<OutputDiscData>> GetDiscs(QueryFieldSet<OutputDiscData> fields, CancellationToken cancellationToken);

		Task<OutputDiscData> GetDisc(int discId, QueryFieldSet<OutputDiscData> fields, CancellationToken cancellationToken);
	}
}
