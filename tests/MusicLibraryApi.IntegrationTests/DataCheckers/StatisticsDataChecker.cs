using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Statistics;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class StatisticsDataChecker : BasicDataChecker<OutputStatisticsData>
	{
		private readonly IDataChecker<RatingSongsData> songsRatingsChecker;

		protected override IEnumerable<Action<OutputStatisticsData, OutputStatisticsData, string>> PropertiesCheckers
		{
			get
			{
				yield return FieldChecker(x => x.ArtistsNumber, nameof(OutputStatisticsData.ArtistsNumber));
				yield return FieldChecker(x => x.DiscArtistsNumber, nameof(OutputStatisticsData.DiscArtistsNumber));
				yield return FieldChecker(x => x.DiscsNumber, nameof(OutputStatisticsData.DiscsNumber));
				yield return FieldChecker(x => x.SongsNumber, nameof(OutputStatisticsData.SongsNumber));
				yield return FieldChecker(x => x.SongsDuration, nameof(OutputStatisticsData.SongsDuration));
				yield return FieldChecker(x => x.PlaybacksDuration, nameof(OutputStatisticsData.PlaybacksDuration));
				yield return FieldChecker(x => x.PlaybacksNumber, nameof(OutputStatisticsData.PlaybacksNumber));
				yield return FieldChecker(x => x.UnheardSongsNumber, nameof(OutputStatisticsData.UnheardSongsNumber));
				yield return FieldChecker(x => x.SongsRatings, songsRatingsChecker, nameof(OutputStatisticsData.SongsRatings));
			}
		}

		public StatisticsDataChecker(IDataChecker<RatingSongsData> songsRatingsChecker)
		{
			this.songsRatingsChecker = songsRatingsChecker ?? throw new ArgumentNullException(nameof(songsRatingsChecker));
		}
	}
}
