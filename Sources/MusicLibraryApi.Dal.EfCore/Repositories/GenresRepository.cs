using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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

		public async Task<int> AddGenre(Genre genre, CancellationToken cancellationToken)
		{
			var genreEntity = mapper.Map<GenreEntity>(genre);

			context.Genres.Add(genreEntity);
			await context.SaveChangesAsync(cancellationToken);

			return genreEntity.Id;
		}

		public async Task<IEnumerable<Genre>> GetAllGenres(CancellationToken cancellationToken)
		{
			return await context.Genres
				.ProjectTo<Genre>(mapper.ConfigurationProvider)
				.ToListAsync(cancellationToken);
		}
	}
}
