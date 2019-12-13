using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using static System.FormattableString;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class GenresRepository : IGenresRepository
	{
		private readonly MusicLibraryDbContext context;

		public GenresRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Task AddGenre(Genre genre, CancellationToken cancellationToken)
		{
			context.Genres.Add(genre);
			return Task.CompletedTask;
		}

		public async Task<IReadOnlyCollection<Genre>> GetAllGenres(CancellationToken cancellationToken)
		{
			return await context.Genres
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Genre>> GetGenres(IEnumerable<int> genreIds, CancellationToken cancellationToken)
		{
			return await context.Genres.Where(g => genreIds.Contains(g.Id))
				.ToListAsync(cancellationToken);
		}

		public async Task<Genre> GetGenre(int genreId, CancellationToken cancellationToken)
		{
			var genre = await context.Genres
				.Where(g => g.Id == genreId)
				.SingleOrDefaultAsync(cancellationToken);

			if (genre == null)
			{
				throw new GenreNotFoundException(Invariant($"The genre with id of {genreId} does not exist"));
			}

			return genre;
		}
	}
}
