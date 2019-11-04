using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class GenresRepository : IGenresRepository
	{
		private readonly MusicLibraryDbContext context;

		public GenresRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<Genre> AddGenre(Genre genre, CancellationToken cancellationToken)
		{
			context.Genres.Add(genre);
			await context.SaveChangesAsync(cancellationToken);

			return genre;
		}

		public async Task<IEnumerable<Genre>> GetAllGenres(CancellationToken cancellationToken)
		{
			return await context.Genres
				.ToListAsync(cancellationToken);
		}
	}
}
