using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Fields;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IArtistsQuery
	{
		Task<IReadOnlyCollection<OutputArtistData>> GetArtists(QueryFieldSet<OutputArtistData> fields, CancellationToken cancellationToken);

		Task<OutputArtistData> GetArtist(int artistId, QueryFieldSet<OutputArtistData> fields, CancellationToken cancellationToken);
	}
}
