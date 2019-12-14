using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts;
using MusicLibraryApi.Client.Fields;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IStatisticsQuery
	{
		Task<OutputStatisticsData> GetStatistics(QueryFieldSet<OutputStatisticsData> fields, CancellationToken cancellationToken);
	}
}
