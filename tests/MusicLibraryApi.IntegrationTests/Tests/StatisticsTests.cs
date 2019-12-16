using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts;
using MusicLibraryApi.Client.Contracts.Statistics;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class StatisticsTests : GraphQLTests
	{
		[TestMethod]
		public async Task StatisticsQuery_ReturnsCorrectStatistics()
		{
			// Arrange

			var songsRatingsNumbers = new[]
			{
				new RatingSongsData { Rating = Rating.R1, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R2, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R3, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R4, SongsNumber = 1, },
				new RatingSongsData { Rating = Rating.R5, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R6, SongsNumber = 1, },
				new RatingSongsData { Rating = Rating.R7, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R8, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R9, SongsNumber = 0, },
				new RatingSongsData { Rating = Rating.R10, SongsNumber = 0, },
				new RatingSongsData { Rating = null, SongsNumber = 1, },
			};

			var expectedStatistics = new OutputStatisticsData
			{
				ArtistsNumber = 2,
				DiscArtistsNumber = 1,
				DiscsNumber = 5,
				SongsNumber = 3,
				SongsDuration = new TimeSpan(0, 12, 49),
				PlaybacksDuration = new TimeSpan(0, 21, 49),
				PlaybacksNumber = 4,
				UnheardSongsNumber = 1,
				SongsRatings = songsRatingsNumbers,
			};

			var client = CreateClient<IStatisticsQuery>();

			// Act

			var receivedStatistics = await client.GetStatistics(StatisticsFields.All + StatisticsFields.SongsRatings(SongsRatingsFields.All), CancellationToken.None);

			// Assert

			AssertData(expectedStatistics, receivedStatistics);
		}
	}
}
