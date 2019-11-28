using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Dal.EfCore.Entities;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class GenresRepository : IGenresRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public GenresRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreateGenre(Genre genre, CancellationToken cancellationToken)
		{
			var genreEntity = mapper.Map<GenreEntity>(genre);

			context.Genres.Add(genreEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsUniqueViolationException())
			{
				throw new DuplicateKeyException($"Failed to add genre '{genre.Name}' to the database", e);
			}

			return genreEntity.Id;
		}

		public async Task<IReadOnlyCollection<Genre>> GetAllGenres(CancellationToken cancellationToken)
		{
			return await context.Genres
				.OrderBy(g => g.Id)
				.Select(g => mapper.Map<Genre>(g))
				.ToListAsync(cancellationToken);
		}
	}
}
