using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Abstractions.Interfaces
{
	public interface ISongTagger
	{
		Task SetTagData(Song song, Disc disc, Artist? artist, Genre? genre, Stream contentStream, CancellationToken cancellationToken);
	}
}
