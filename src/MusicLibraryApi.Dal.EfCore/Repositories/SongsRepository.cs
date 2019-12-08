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
using static System.FormattableString;

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

		public async Task<int> CreateSong(int discId, int? artistId, int? genreId, Song song, CancellationToken cancellationToken)
		{
			var songEntity = mapper.Map<SongEntity>(song);
			songEntity.Disc = await FindDisc(discId, cancellationToken);
			songEntity.Artist = artistId == null ? null : await FindArtist(artistId.Value, cancellationToken);
			songEntity.Genre = genreId == null ? null : await FindGenre(genreId.Value, cancellationToken);

			context.Songs.Add(songEntity);
			await context.SaveChangesAsync(cancellationToken);

			return songEntity.Id;
		}

		public async Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken)
		{
			return await context.Songs
				.Select(d => mapper.Map<Song>(d))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetDiscSongs(int discId, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => s.Disc.Id == discId)
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetGenreSongs(int genreId, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => s.Genre != null && s.Genre.Id == genreId)
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}

		public async Task<DiscEntity> FindDisc(int discId, CancellationToken cancellationToken)
		{
			var discEntity = await context.Discs.SingleOrDefaultAsync(d => d.Id == discId, cancellationToken);
			if (discEntity == null)
			{
				throw new DiscNotFoundException(Invariant($"The folder with id of {discId} does not exist"));
			}

			return discEntity;
		}

		public async Task<ArtistEntity> FindArtist(int artistId, CancellationToken cancellationToken)
		{
			var artistEntity = await context.Artists.SingleOrDefaultAsync(d => d.Id == artistId, cancellationToken);
			if (artistEntity == null)
			{
				throw new ArtistNotFoundException(Invariant($"The artist with id of {artistId} does not exist"));
			}

			return artistEntity;
		}

		public async Task<GenreEntity> FindGenre(int genreId, CancellationToken cancellationToken)
		{
			var genreEntity = await context.Genres.SingleOrDefaultAsync(d => d.Id == genreId, cancellationToken);
			if (genreEntity == null)
			{
				throw new GenreNotFoundException(Invariant($"The genre with id of {genreId} does not exist"));
			}

			return genreEntity;
		}
	}
}
