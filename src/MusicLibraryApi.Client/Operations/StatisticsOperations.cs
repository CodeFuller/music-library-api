using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Statistics;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.Client.Operations
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class StatisticsOperations : BasicQuery, IStatisticsQuery
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public StatisticsOperations(IHttpClientFactory httpClientFactory, ILogger<BasicQuery> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<OutputStatisticsData> GetStatistics(QueryFieldSet<OutputStatisticsData> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<OutputStatisticsData, OutputStatisticsData>("statistics", fields, cancellationToken);
		}
	}
}
