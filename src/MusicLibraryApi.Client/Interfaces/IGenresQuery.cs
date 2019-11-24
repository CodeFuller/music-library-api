using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Genres;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IGenresQuery
	{
		Task<IReadOnlyCollection<OutputGenreData>> GetGenres(QueryFieldSet fields, CancellationToken cancellationToken);
	}
}
