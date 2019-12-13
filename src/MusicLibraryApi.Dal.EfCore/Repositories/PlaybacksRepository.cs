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
	public class PlaybacksRepository : IPlaybacksRepository
	{
		private readonly MusicLibraryDbContext context;

		public PlaybacksRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<int> CreatePlayback(Playback playback, CancellationToken cancellationToken)
		{
			context.Playbacks.Add(playback);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException())
			{
				throw new SongNotFoundException(Invariant($"The song with id of {playback.SongId} does not exist"));
			}

			return playback.Id;
		}

		public async Task<IReadOnlyCollection<Playback>> GetAllPlaybacks(CancellationToken cancellationToken)
		{
			return await context.Playbacks
				.ToListAsync(cancellationToken);
		}

		public async Task<Playback> GetPlayback(int playbackId, CancellationToken cancellationToken)
		{
			var playback = await context.Playbacks
				.Where(p => p.Id == playbackId)
				.SingleOrDefaultAsync(cancellationToken);

			if (playback == null)
			{
				throw new PlaybackNotFoundException(Invariant($"The playback with id of {playbackId} does not exist"));
			}

			return playback;
		}

		public async Task<IReadOnlyCollection<Playback>> GetPlaybacksBySongIds(IEnumerable<int> songIds, CancellationToken cancellationToken)
		{
			return await context.Playbacks.Where(p => songIds.Contains(p.SongId))
				.ToListAsync(cancellationToken);
		}
	}
}
