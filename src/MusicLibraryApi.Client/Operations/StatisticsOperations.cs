using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.Client.Operations
{
	public class StatisticsOperations : BasicQuery, IStatisticsQuery
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
