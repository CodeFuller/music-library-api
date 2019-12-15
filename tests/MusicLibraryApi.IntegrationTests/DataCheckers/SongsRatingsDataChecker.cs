using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Statistics;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class SongsRatingsDataChecker : BasicDataChecker<RatingSongsData>
	{
		protected override IEnumerable<Action<RatingSongsData, RatingSongsData, string>> PropertiesCheckers
		{
			get
			{
				yield return FieldChecker(x => x.Rating, nameof(RatingSongsData.Rating));
				yield return FieldChecker(x => x.SongsNumber, nameof(RatingSongsData.SongsNumber));
			}
		}
	}
}
