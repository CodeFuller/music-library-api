using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IGenresRepository
	{
		Task<Genre> AddGenre(Genre genre, CancellationToken cancellationToken);

		Task<IEnumerable<Genre>> GetAllGenres(CancellationToken cancellationToken);
	}
}
