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
	public class PlaybacksService : IPlaybacksService
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly IPlaybacksRepository repository;

		private readonly ILogger<PlaybacksService> logger;

		public PlaybacksService(IUnitOfWork unitOfWork, ILogger<PlaybacksService> logger)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.repository = unitOfWork.PlaybacksRepository;
		}

		public async Task<int> CreatePlayback(Playback playback, CancellationToken cancellationToken)
		{
			var playbackTime = playback.PlaybackTime;

			Song song;

			try
			{
				song = await unitOfWork.SongsRepository.GetSong(playback.SongId, cancellationToken);
			}
			catch (SongNotFoundException e)
			{
				throw e.Handle(playback.SongId, logger);
			}

			if (song.LastPlaybackTime != null && playbackTime <= song.LastPlaybackTime)
			{
				throw new ServiceOperationFailedException($"Can not add earlier playback for the song: {playbackTime:yyyy.MM.dd HH:mm:ss} <= {song.LastPlaybackTime:yyyy.MM.dd HH:mm:ss}");
			}

			await repository.AddPlayback(playback, cancellationToken);

			song.PlaybacksCount += 1;
			song.LastPlaybackTime = playbackTime;

			await unitOfWork.Commit(cancellationToken);

			return playback.Id;
		}

		public async Task<IReadOnlyCollection<Playback>> GetAllPlaybacks(CancellationToken cancellationToken)
		{
			var playbacks = await repository.GetAllPlaybacks(cancellationToken);

			return playbacks
				.OrderBy(p => p.PlaybackTime).ToList();
		}

		public async Task<Playback> GetPlayback(int playbackId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetPlayback(playbackId, cancellationToken);
			}
			catch (PlaybackNotFoundException e)
			{
				throw e.Handle(playbackId, logger);
			}
		}

		public async Task<ILookup<int, Playback>> GetPlaybacksBySongIds(IEnumerable<int> songIds, CancellationToken cancellationToken)
		{
			var playbacks = await repository.GetPlaybacksBySongIds(songIds, cancellationToken);
			return playbacks
				.OrderBy(p => p.PlaybackTime)
				.ToLookup(p => p.SongId);
		}
	}
}
