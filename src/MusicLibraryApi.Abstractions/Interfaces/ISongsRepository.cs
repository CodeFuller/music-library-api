using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface ISongsRepository
	{
		Task<IReadOnlyCollection<Song>> GetDiscSongs(int discId, CancellationToken cancellationToken);
	}
}
