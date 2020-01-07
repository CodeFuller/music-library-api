using System.Threading;
using System.Threading.Tasks;

namespace MusicLibraryApi.Logic.Interfaces
{
	public interface IChecksumCalculator
	{
		Task<uint> CalculateChecksum(byte[] content, CancellationToken cancellationToken);
	}
}
