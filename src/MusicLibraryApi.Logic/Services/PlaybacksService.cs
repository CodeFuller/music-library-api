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
	public class PlaybacksService : IPlaybacksService
	{
		private readonly IPlaybacksRepository repository;

		private readonly ILogger<PlaybacksService> logger;

		public PlaybacksService(IPlaybacksRepository repository, ILogger<PlaybacksService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> CreatePlayback(Playback playback, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.CreatePlayback(playback, cancellationToken);
			}
			catch (SongNotFoundException e)
			{
				throw e.Handle(playback.SongId, logger);
			}
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
