using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Logic.Extensions;

namespace MusicLibraryApi.Logic.Services
{
	public class StatisticsService : IStatisticsService
	{
		private readonly IUnitOfWork unitOfWork;

		public StatisticsService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<int> GetArtistsNumber(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Where(s => !s.IsDeleted)
				.Select(s => s.ArtistId)
				.Where(artistId => artistId != null)
				.Distinct()
				.Count();
		}

		public async Task<int> GetDiscArtistsNumber(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Where(s => !s.IsDeleted)
				.GroupBy(s => s.DiscId)
				.Select(discSongs => discSongs.Select(s => s.ArtistId).Distinct())
				.Where(discSongArtists => discSongArtists.Count() == 1)
				.Select(discSongArtists => discSongArtists.Single())
				.Where(artistId => artistId != null)
				.Distinct()
				.Count();
		}

		public async Task<int> GetDiscsNumber(CancellationToken cancellationToken)
		{
			var discs = await unitOfWork.DiscsRepository.GetAllDiscs(cancellationToken);
			return discs
				.Count(d => !d.IsDeleted);
		}

		public async Task<int> GetSongsNumber(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Count(s => !s.IsDeleted);
		}

		public async Task<TimeSpan> GetSongsDuration(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Where(s => !s.IsDeleted)
				.Select(s => s.Duration)
				.Sum();
		}

		public async Task<TimeSpan> GetPlaybacksDuration(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Select(s => s.Duration * s.PlaybacksCount)
				.Sum();
		}

		public async Task<int> GetPlaybacksNumber(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Select(s => s.PlaybacksCount)
				.Sum();
		}

		public async Task<int> GetUnheardSongsNumber(CancellationToken cancellationToken)
		{
			var songs = await unitOfWork.SongsRepository.GetAllSongs(cancellationToken);
			return songs
				.Where(s => !s.IsDeleted)
				.Count(s => s.PlaybacksCount == 0);
		}
	}
}
