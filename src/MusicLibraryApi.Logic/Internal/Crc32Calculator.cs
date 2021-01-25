using System.Threading;
using System.Threading.Tasks;
using Force.Crc32;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.Logic.Internal
{
	internal class Crc32Calculator : IChecksumCalculator
	{
		public Task<uint> CalculateChecksum(byte[] content, CancellationToken cancellationToken)
		{
			var checksum = Crc32Algorithm.Compute(content);
			return Task.FromResult(checksum);
		}
	}
}
