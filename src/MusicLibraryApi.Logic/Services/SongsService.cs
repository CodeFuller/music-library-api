﻿using System;
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

		public async Task<int> CreateSong(int discId, int? artistId, int? genreId, Song song, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.CreateSong(discId, artistId, genreId, song, cancellationToken);
			}
			catch (DiscNotFoundException e)
			{
				throw e.Handle(discId, logger);
			}
			catch (ArtistNotFoundException e)
			{
				throw e.Handle(artistId, logger);
			}
			catch (GenreNotFoundException e)
			{
				throw e.Handle(genreId, logger);
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

		public async Task<IReadOnlyCollection<Song>> GetDiscSongs(int discId, CancellationToken cancellationToken)
		{
			try
			{
				var discs = await repository.GetDiscSongs(discId, cancellationToken);
				return discs
					.Where(s => !s.IsDeleted)
					.OrderBy(f => f.TrackNumber == null)
					.ThenBy(d => d.TrackNumber)
					.ThenBy(d => d.Title)
					.ToList();
			}
			catch (DiscNotFoundException e)
			{
				throw e.Handle(discId, logger);
			}
		}
	}
}
