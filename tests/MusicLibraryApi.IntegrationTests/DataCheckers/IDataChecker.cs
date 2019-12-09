using System.Collections.Generic;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public interface IDataChecker<in TData>
		where TData : class
	{
		void CheckData(TData? expected, TData? actual, string dataPath);

		void CheckData(IReadOnlyCollection<TData?>? expected, IReadOnlyCollection<TData?>? actual, string dataPath);
	}
}
