using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Client.Contracts.Discs;

namespace MusicLibraryApi.Client.Interfaces
{
	public interface IDiscsMutation
	{
		Task<int> CreateDisc(int? folderId, InputDiscData discData, CancellationToken cancellationToken);
	}
}
