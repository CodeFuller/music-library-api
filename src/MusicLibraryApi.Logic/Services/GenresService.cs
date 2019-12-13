using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Extensions;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Services
{
	public class GenresService : IGenresService
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly IGenresRepository repository;

		private readonly ILogger<GenresService> logger;

		public GenresService(IUnitOfWork unitOfWork, ILogger<GenresService> logger)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.repository = unitOfWork.GenresRepository;
		}

		public async Task<int> CreateGenre(Genre genre, CancellationToken cancellationToken)
		{
			try
			{
				await repository.AddGenre(genre, cancellationToken);
				await unitOfWork.Commit(cancellationToken);

				return genre.Id;
			}
			catch (DuplicateKeyException e)
			{
				logger.LogError(e, "Genre {GenreName} already exists", genre.Name);
				throw new ServiceOperationFailedException(Invariant($"Genre '{genre.Name}' already exists"), e);
			}
		}

		public async Task<IReadOnlyCollection<Genre>> GetAllGenres(CancellationToken cancellationToken)
		{
			var genres = await repository.GetAllGenres(cancellationToken);
			return genres.OrderBy(g => g.Name).ToList();
		}

		public async Task<IDictionary<int, Genre>> GetGenres(IEnumerable<int> genreIds, CancellationToken cancellationToken)
		{
			var genres = await repository.GetGenres(genreIds, cancellationToken);
			return genres.ToDictionary(g => g.Id);
		}

		public async Task<Genre> GetGenre(int genreId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetGenre(genreId, cancellationToken);
			}
			catch (GenreNotFoundException e)
			{
				throw e.Handle(genreId, logger);
			}
		}
	}
}
