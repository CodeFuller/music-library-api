using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Discs;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IDiscsMutation
	{
		Task<int> CreateDisc(InputDiscData discData, CancellationToken cancellationToken);

		Task<int> CreateDisc(InputDiscData discData, Stream coverContent, CancellationToken cancellationToken);
	}
}
