using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Discs;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IDiscsMutation
	{
		Task<int> CreateDisc(InputDiscData discData, CancellationToken cancellationToken);
	}
}
