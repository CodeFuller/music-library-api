using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class SongsRepository : ISongsRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public SongsRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<IReadOnlyCollection<Song>> GetDiscSongs(int discId, CancellationToken cancellationToken)
		{
			return await context.Songs
				.Where(s => s.Disc.Id == discId)
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}
	}
}
