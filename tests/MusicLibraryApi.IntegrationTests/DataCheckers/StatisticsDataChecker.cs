using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class StatisticsDataChecker : BasicDataChecker<OutputStatisticsData>
	{
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
			}
		}
	}
}
