using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IGenresQuery
	{
		Task<IReadOnlyCollection<OutputGenreData>> GetGenres(QueryFieldSet<GenreQuery> fields, CancellationToken cancellationToken);

		Task<OutputGenreData> GetGenre(int genreId, QueryFieldSet<GenreQuery> fields, CancellationToken cancellationToken);
	}
}
