﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IGenresService
	{
		Task<int> CreateGenre(Genre genre, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<Genre>> GetAllGenres(CancellationToken cancellationToken);

		Task<IDictionary<int, Genre>> GetGenres(IEnumerable<int> genreIds, CancellationToken cancellationToken);

		Task<Genre> GetGenre(int genreId, CancellationToken cancellationToken);
	}
}
