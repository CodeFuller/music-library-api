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
				new RatingSongsData(Rating.R1, 0),
				new RatingSongsData(Rating.R2, 0),
				new RatingSongsData(Rating.R3, 0),
				new RatingSongsData(Rating.R4, 1),
				new RatingSongsData(Rating.R5, 0),
				new RatingSongsData(Rating.R6, 1),
				new RatingSongsData(Rating.R7, 0),
				new RatingSongsData(Rating.R8, 0),
				new RatingSongsData(Rating.R9, 0),
				new RatingSongsData(Rating.R10, 0),
				new RatingSongsData(null, 1),
			};

			var expectedStatistics = new OutputStatisticsData(artistsNumber: 2, discArtistsNumber: 1, discsNumber: 5, songsNumber: 3, songsDuration: new TimeSpan(0, 12, 49),
				playbacksDuration: new TimeSpan(0, 21, 49), playbacksNumber: 4, unheardSongsNumber: 1, songsRatings: songsRatingsNumbers);

			var client = CreateClient<IStatisticsQuery>();

			// Act

			var receivedStatistics = await client.GetStatistics(StatisticsFields.All + StatisticsFields.SongsRatings(SongsRatingsFields.All), CancellationToken.None);

			// Assert

			AssertData(expectedStatistics, receivedStatistics);
		}
	}
}
