using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Services
{
	public class GenresService : IGenresService
	{
		private readonly IGenresRepository repository;

		private readonly ILogger<GenresService> logger;

		public GenresService(IGenresRepository repository, ILogger<GenresService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> CreateGenre(Genre genre, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.CreateGenre(genre, cancellationToken);
			}
			catch (DuplicateKeyException e)
			{
				logger.LogError(e, "Genre {GenreName} already exists", genre.Name);
				throw new ServiceOperationFailedException(Invariant($"Genre '{genre.Name}' already exists"), e);
			}
		}

		public Task<IReadOnlyCollection<Genre>> GetAllGenres(CancellationToken cancellationToken)
		{
			return repository.GetAllGenres(cancellationToken);
		}
	}
}
