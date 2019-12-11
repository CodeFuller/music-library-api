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

namespace MusicLibraryApi.Logic.Services
{
	public class SongsService : ISongsService
	{
		private readonly ISongsRepository repository;

		private readonly ILogger<SongsService> logger;

		public SongsService(ISongsRepository repository, ILogger<SongsService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> CreateSong(Song song, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.CreateSong(song, cancellationToken);
			}
			catch (DiscNotFoundException e)
			{
				throw e.Handle(song.DiscId, logger);
			}
			catch (ArtistNotFoundException e)
			{
				throw e.Handle(song.ArtistId, logger);
			}
			catch (GenreNotFoundException e)
			{
				throw e.Handle(song.GenreId, logger);
			}
		}

		public async Task<IReadOnlyCollection<Song>> GetAllSongs(CancellationToken cancellationToken)
		{
			var discs = await repository.GetAllSongs(cancellationToken);

			// There is no meaningful sorting for all songs. We sort them by id here mostly for steady IT baselines.
			return discs
				.Where(d => !d.IsDeleted)
				.OrderBy(d => d.Id).ToList();
		}

		public async Task<Song> GetSong(int songId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetSong(songId, cancellationToken);
			}
			catch (SongNotFoundException e)
			{
				throw e.Handle(songId, logger);
			}
		}

		public async Task<ILookup<int, Song>> GetSongsByDiscIds(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			var songs = await repository.GetSongsByDiscIds(discIds, cancellationToken);
			return songs.Select(s => s)
				.Where(s => !s.IsDeleted)
				.OrderBy(s => s.TrackNumber == null)
				.ThenBy(s => s.TrackNumber)
				.ThenBy(s => s.Title)
				.ToLookup(s => s.DiscId);
		}

		public async Task<ILookup<int, Song>> GetSongsByArtistIds(IEnumerable<int> artistIds, CancellationToken cancellationToken)
		{
			var songs = await repository.GetSongsByArtistIds(artistIds, cancellationToken);
			return FilterAndOrderMixedSongs(songs)
				.ToLookup(s => s.ArtistId ?? 0);
		}

		public async Task<ILookup<int, Song>> GetSongsByGenreIds(IEnumerable<int> genreIds, CancellationToken cancellationToken)
		{
			var songs = await repository.GetSongsByGenreIds(genreIds, cancellationToken);
			return FilterAndOrderMixedSongs(songs)
				.ToLookup(s => s.GenreId ?? 0);
		}

		private IReadOnlyCollection<Song> FilterAndOrderMixedSongs(IReadOnlyCollection<Song> songs)
		{
			return songs
				.Where(s => !s.IsDeleted)
				.OrderBy(s => s.Id)
				.ToList();
		}
	}
}
