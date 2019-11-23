using System.Collections.Generic;
using System.Threading;
using MusicLibraryApi.Client.Contracts.Output;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IGenresQuery
	{
		IAsyncEnumerable<GenreDto> GetGenres(QueryFieldSet fields, CancellationToken cancellationToken);
	}
}
