using System.Threading;
using System.Threading.Tasks;
using Force.Crc32;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.Logic.Internal
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class Crc32Calculator : IChecksumCalculator
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public Task<uint> CalculateChecksum(byte[] content, CancellationToken cancellationToken)
		{
			var checksum = Crc32Algorithm.Compute(content);
			return Task.FromResult(checksum);
		}
	}
}
