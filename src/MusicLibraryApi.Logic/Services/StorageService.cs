using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Services
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class StorageService : IStorageService
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		private const string PathSeparator = "/";

		private readonly IUnitOfWork unitOfWork;

		private readonly IContentStorage contentStorage;

		private readonly IChecksumCalculator checksumCalculator;

		public StorageService(IUnitOfWork unitOfWork, IContentStorage contentStorage, IChecksumCalculator checksumCalculator)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.contentStorage = contentStorage ?? throw new ArgumentNullException(nameof(contentStorage));
			this.checksumCalculator = checksumCalculator ?? throw new ArgumentNullException(nameof(checksumCalculator));
		}

		public async Task CreateFolder(Folder folder, CancellationToken cancellationToken)
		{
			var folderPath = await GetFolderPath(folder, cancellationToken);
			await contentStorage.CreateFolder(folderPath, cancellationToken);
		}

		public async Task CreateDisc(Disc disc, CancellationToken cancellationToken)
		{
			var discFolderPath = await GetDiscFolderPath(disc, cancellationToken);
			await contentStorage.CreateFolder(discFolderPath, cancellationToken);
		}

		public async Task StoreSong(Song song, Stream contentStream, CancellationToken cancellationToken)
		{
			var songFilePath = await GetSongFilePath(song, cancellationToken);
			var content = await ReadContent(contentStream, cancellationToken);
			await contentStorage.StoreContent(songFilePath, content, cancellationToken);

			// Enriching the song with content info
			song.Size = content.LongLength;
			song.Checksum = await checksumCalculator.CalculateChecksum(content, cancellationToken);
		}

		public async Task RollbackFolderCreation(Folder folder, CancellationToken cancellationToken)
		{
			var folderPath = await GetFolderPath(folder, cancellationToken);
			await contentStorage.DeleteEmptyFolder(folderPath, cancellationToken);
		}

		public async Task RollbackDiscCreation(Disc disc, CancellationToken cancellationToken)
		{
			var discFolderPath = await GetDiscFolderPath(disc, cancellationToken);
			await contentStorage.DeleteEmptyFolder(discFolderPath, cancellationToken);
		}

		public async Task RollbackSongCreation(Song song, CancellationToken cancellationToken)
		{
			var songFilePath = await GetSongFilePath(song, cancellationToken);
			await contentStorage.DeleteContent(songFilePath, cancellationToken);
		}

		private async Task<string> GetFolderPath(Folder folder, CancellationToken cancellationToken)
		{
			if (folder.ParentFolderId == null)
			{
				throw new InvalidOperationException("Parent folder id is not set");
			}

			var parentFolderPathParts = await GetFolderPathParts(folder.ParentFolderId.Value, cancellationToken);
			return CombinePathParts(parentFolderPathParts.Concat(new[] { folder.Name }));
		}

		private async Task<string> GetDiscFolderPath(Disc disc, CancellationToken cancellationToken)
		{
			var discPathParts = await GetDiscPathParts(disc, cancellationToken);
			return CombinePathParts(discPathParts);
		}

		private async Task<string> GetSongFilePath(Song song, CancellationToken cancellationToken)
		{
			var discsRepository = unitOfWork.DiscsRepository;
			var disc = await discsRepository.GetDisc(song.DiscId, cancellationToken);

			var discPathParts = await GetDiscPathParts(disc, cancellationToken);
			var songPathParts = discPathParts.Concat(new[] { song.TreeTitle });
			return CombinePathParts(songPathParts);
		}

		private async Task<IEnumerable<string>> GetDiscPathParts(Disc disc, CancellationToken cancellationToken)
		{
			var parentFolderPathParts = await GetFolderPathParts(disc.FolderId, cancellationToken);
			return parentFolderPathParts.Concat(new[] { disc.TreeTitle });
		}

		private async Task<IReadOnlyCollection<string>> GetFolderPathParts(int folderId, CancellationToken cancellationToken)
		{
			var foldersRepository = unitOfWork.FoldersRepository;

			var parts = new List<string>();

			for (int? currFolderId = folderId; currFolderId != null;)
			{
				var currFolder = await foldersRepository.GetFolder(currFolderId.Value, cancellationToken);
				parts.Add(currFolder.Name);

				currFolderId = currFolder.ParentFolderId;
			}

			parts.Reverse();

			return parts;
		}

		private static string CombinePathParts(IEnumerable<string> parts)
		{
			// We skip the first root folder.
			return parts
				.Skip(1)
				.Select(SafePathName)
				.Aggregate((currPath, currPart) => Invariant($"{currPath}{PathSeparator}{currPart}"));
		}

		private static string SafePathName(string name)
		{
			// TBD: Remove unsafe path characters from the name.
			return name;
		}

		private static async Task<byte[]> ReadContent(Stream contentStream, CancellationToken cancellationToken)
		{
			await using var memoryStream = new MemoryStream();
			await contentStream.CopyToAsync(memoryStream, cancellationToken);

			return memoryStream.ToArray();
		}
	}
}
