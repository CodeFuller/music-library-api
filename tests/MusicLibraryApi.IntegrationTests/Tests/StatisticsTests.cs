using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Contracts;
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

			var expectedStatistics = new OutputStatisticsData(artistsNumber: 2, discArtistsNumber: 2, discsNumber: 5, songsNumber: 3,
				songsDuration: new TimeSpan(0, 12, 49), playbacksDuration: new TimeSpan(0, 21, 49), playbacksNumber: 4, unheardSongsNumber: 1);

			var client = CreateClient<IStatisticsQuery>();

			// Act

			var receivedStatistics = await client.GetStatistics(StatisticsFields.All, CancellationToken.None);

			// Assert

			AssertData(expectedStatistics, receivedStatistics);
		}
	}
}
