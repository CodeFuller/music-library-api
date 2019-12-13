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

		public async Task<int> CreateSong(Song song, CancellationToken cancellationToken)
		{
			var songEntity = mapper.Map<SongEntity>(song);
			context.Songs.Add(songEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException(MusicLibraryDbContext.SongDiscForeignKeyName))
			{
				throw new DiscNotFoundException(Invariant($"The disc with id of {song.DiscId} does not exist"));
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException(MusicLibraryDbContext.SongArtistForeignKeyName))
			{
				throw new ArtistNotFoundException(Invariant($"The artist with id of {song.ArtistId} does not exist"));
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException(MusicLibraryDbContext.SongGenreForeignKeyName))
			{
				throw new GenreNotFoundException(Invariant($"The genre with id of {song.GenreId} does not exist"));
			}

			return songEntity.Id;
		}

		public async Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken)
		{
			return await context.Songs
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongs(IEnumerable<int> songIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => songIds.Contains(s.Id))
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}

		public async Task<Song> GetSong(int songId, CancellationToken cancellationToken)
		{
			var songEntity = await context.Songs
				.Where(s => s.Id == songId)
				.SingleOrDefaultAsync(cancellationToken);

			if (songEntity == null)
			{
				throw new SongNotFoundException(Invariant($"The song with id of {songId} does not exist"));
			}

			return mapper.Map<Song>(songEntity);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongsByDiscIds(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => discIds.Contains(s.DiscId))
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongsByArtistIds(IEnumerable<int> artistIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => s.ArtistId != null && artistIds.Contains(s.ArtistId.Value))
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongsByGenreIds(IEnumerable<int> genreIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => s.GenreId != null && genreIds.Contains(s.GenreId.Value))
				.Select(s => mapper.Map<Song>(s))
				.ToListAsync(cancellationToken);
		}
	}
}
