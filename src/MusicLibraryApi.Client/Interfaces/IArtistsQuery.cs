using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IArtistsQuery
	{
		Task<IReadOnlyCollection<OutputArtistData>> GetArtists(QueryFieldSet<ArtistQuery> fields, CancellationToken cancellationToken);
	}
}
