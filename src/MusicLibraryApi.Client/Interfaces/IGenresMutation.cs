using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Genres;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IGenresMutation
	{
		Task<int> CreateGenre(InputGenreData genreData, CancellationToken cancellationToken);
	}
}
