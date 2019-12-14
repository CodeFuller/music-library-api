using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface IStatisticsService
	{
		Task<int> GetArtistsNumber(CancellationToken cancellationToken);

		Task<int> GetDiscArtistsNumber(CancellationToken cancellationToken);

		Task<int> GetDiscsNumber(CancellationToken cancellationToken);

		Task<int> GetSongsNumber(CancellationToken cancellationToken);

		Task<TimeSpan> GetSongsDuration(CancellationToken cancellationToken);

		Task<TimeSpan> GetPlaybacksDuration(CancellationToken cancellationToken);

		Task<int> GetPlaybacksNumber(CancellationToken cancellationToken);

		Task<int> GetUnheardSongsNumber(CancellationToken cancellationToken);
	}
}
