using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Extensions;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.Logic.Services
{
	public class SongsService : ISongsService
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly ISongsRepository repository;

		private readonly IStorageService storageService;

		private readonly ILogger<SongsService> logger;

		public SongsService(IUnitOfWork unitOfWork, IStorageService storageService, ILogger<SongsService> logger)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.repository = unitOfWork.SongsRepository;
		}

		public async Task<int> CreateSong(Song song, Stream contentStream, CancellationToken cancellationToken)
		{
			// The storage changes should be made before saving song in DB, because StoreSong() enrhiches song with content info.
			await storageService.StoreSong(song, contentStream, cancellationToken);

			// TBD: Rollback storage changes on error
			return await AddSongToRepository(song, cancellationToken);
		}

		public Task<int> CreateDeletedSong(Song song, CancellationToken cancellationToken)
		{
			return AddSongToRepository(song, cancellationToken);
		}

		private async Task<int> AddSongToRepository(Song song, CancellationToken cancellationToken)
		{
			await repository.AddSong(song, cancellationToken);

			try
			{
				await unitOfWork.Commit(cancellationToken);
				return song.Id;
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
			var songs = await repository.GetAllSongs(cancellationToken);

			// There is no meaningful sorting for all songs. We sort them by id here mostly for steady IT baselines.
			return songs
				.Where(s => !s.IsDeleted)
				.OrderBy(s => s.Id).ToList();
		}

		public async Task<IDictionary<int, Song>> GetSongs(IEnumerable<int> songIds, CancellationToken cancellationToken)
		{
			var songs = await repository.GetSongs(songIds, cancellationToken);
			return songs.ToDictionary(s => s.Id);
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

		private static IEnumerable<Song> FilterAndOrderMixedSongs(IEnumerable<Song> songs)
		{
			return songs
				.Where(s => !s.IsDeleted)
				.OrderBy(s => s.Id);
		}
	}
}
