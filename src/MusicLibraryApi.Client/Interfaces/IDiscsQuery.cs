using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IDiscsQuery
	{
		Task<IReadOnlyCollection<OutputDiscData>> GetDiscs(QueryFieldSet<DiscQuery> fields, CancellationToken cancellationToken);

		Task<OutputDiscData> GetDisc(int discId, QueryFieldSet<DiscQuery> fields, CancellationToken cancellationToken);
	}
}
