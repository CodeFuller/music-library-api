using System.Collections.Generic;
using System.Threading;
using MusicLibraryApi.Client.Contracts.Genres;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IGenresQuery
	{
		IAsyncEnumerable<OutputGenreData> GetGenres(QueryFieldSet fields, CancellationToken cancellationToken);
	}
}
