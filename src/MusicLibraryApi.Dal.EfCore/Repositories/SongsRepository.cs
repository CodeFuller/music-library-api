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
	public class SongsRepository : ISongsRepository
	{
		private readonly MusicLibraryDbContext context;

		public SongsRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<int> CreateSong(Song song, CancellationToken cancellationToken)
		{
			context.Songs.Add(song);

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

			return song.Id;
		}

		public async Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken)
		{
			return await context.Songs
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongs(IEnumerable<int> songIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => songIds.Contains(s.Id))
				.ToListAsync(cancellationToken);
		}

		public async Task<Song> GetSong(int songId, CancellationToken cancellationToken)
		{
			var song = await context.Songs
				.Where(s => s.Id == songId)
				.SingleOrDefaultAsync(cancellationToken);

			if (song == null)
			{
				throw new SongNotFoundException(Invariant($"The song with id of {songId} does not exist"));
			}

			return song;
		}

		public async Task<IReadOnlyCollection<Song>> GetSongsByDiscIds(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => discIds.Contains(s.DiscId))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongsByArtistIds(IEnumerable<int> artistIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => s.ArtistId != null && artistIds.Contains(s.ArtistId.Value))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Song>> GetSongsByGenreIds(IEnumerable<int> genreIds, CancellationToken cancellationToken)
		{
			return await context.Songs.Where(s => s.GenreId != null && genreIds.Contains(s.GenreId.Value))
				.ToListAsync(cancellationToken);
		}
	}
}
