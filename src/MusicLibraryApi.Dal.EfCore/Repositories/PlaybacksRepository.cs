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
	public class PlaybacksRepository : IPlaybacksRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public PlaybacksRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreatePlayback(Playback playback, CancellationToken cancellationToken)
		{
			var playbackEntity = mapper.Map<PlaybackEntity>(playback);
			context.Playbacks.Add(playbackEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException())
			{
				throw new SongNotFoundException(Invariant($"The song with id of {playback.SongId} does not exist"));
			}

			return playbackEntity.Id;
		}

		public async Task<IReadOnlyCollection<Playback>> GetAllPlaybacks(CancellationToken cancellationToken)
		{
			return await context.Playbacks
				.Select(p => mapper.Map<Playback>(p))
				.ToListAsync(cancellationToken);
		}

		public async Task<Playback> GetPlayback(int playbackId, CancellationToken cancellationToken)
		{
			var playbackEntity = await context.Playbacks
				.Where(p => p.Id == playbackId)
				.SingleOrDefaultAsync(cancellationToken);

			if (playbackEntity == null)
			{
				throw new PlaybackNotFoundException(Invariant($"The playback with id of {playbackId} does not exist"));
			}

			return mapper.Map<Playback>(playbackEntity);
		}

		public async Task<IReadOnlyCollection<Playback>> GetPlaybacksBySongIds(IEnumerable<int> songIds, CancellationToken cancellationToken)
		{
			return await context.Playbacks.Where(p => songIds.Contains(p.SongId))
				.Select(p => mapper.Map<Playback>(p))
				.ToListAsync(cancellationToken);
		}
	}
}
