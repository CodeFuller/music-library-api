using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.Logic.Internal
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class StorageService : IStorageService
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly IContentStorage contentStorage;

		private readonly ISongTagger songTagger;

		private readonly IChecksumCalculator checksumCalculator;

		public StorageService(IUnitOfWork unitOfWork, IContentStorage contentStorage, ISongTagger songTagger, IChecksumCalculator checksumCalculator)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.contentStorage = contentStorage ?? throw new ArgumentNullException(nameof(contentStorage));
			this.songTagger = songTagger ?? throw new ArgumentNullException(nameof(songTagger));
			this.checksumCalculator = checksumCalculator ?? throw new ArgumentNullException(nameof(checksumCalculator));
		}

		public async Task CreateFolder(Folder folder, CancellationToken cancellationToken)
		{
			var folderPathParts = await GetFolderPathParts(folder, cancellationToken);
			await contentStorage.CreateFolder(folderPathParts, cancellationToken);
		}

		public async Task CreateDisc(Disc disc, CancellationToken cancellationToken)
		{
			var discFolderPathParts = await GetDiscFolderPathParts(disc, cancellationToken);
			await contentStorage.CreateFolder(discFolderPathParts, cancellationToken);
		}

		public async Task StoreDiscCover(Disc disc, Stream coverContentStream, CancellationToken cancellationToken)
		{
			var discFolderPathParts = await GetDiscFolderPathParts(disc, cancellationToken);

			await using var memoryStream = new MemoryStream();
			await coverContentStream.CopyToAsync(memoryStream, cancellationToken);
			var content = memoryStream.ToArray();

			var coverFileName = GetCoverFileName(memoryStream);

			var coverFilePathParts = discFolderPathParts.Concat(new[] { coverFileName });

			await contentStorage.StoreContent(coverFilePathParts, content, cancellationToken);

			disc.CoverFileName = coverFileName;
			disc.CoverSize = content.LongLength;
			disc.CoverChecksum = await checksumCalculator.CalculateChecksum(content, cancellationToken);
		}

		public async Task StoreSong(Song song, Stream contentStream, CancellationToken cancellationToken)
		{
			var songFilePathParts = await GetSongFilePathParts(song, cancellationToken);

			// Loading all data required required for song tagging.
			var disc = await unitOfWork.DiscsRepository.GetDisc(song.DiscId, cancellationToken);
			var artist = song.ArtistId.HasValue ? await unitOfWork.ArtistsRepository.GetArtist(song.ArtistId.Value, cancellationToken) : null;
			var genre = song.GenreId.HasValue ? await unitOfWork.GenresRepository.GetGenre(song.GenreId.Value, cancellationToken) : null;

			// We do not know the capabilities of input stream (i.e. CanWrite, CanSeek properties).
			// That's why we open MemoryStream on top of it.
			await using var memoryStream = new MemoryStream();
			await contentStream.CopyToAsync(memoryStream, cancellationToken);

			await songTagger.SetTagData(song, disc, artist, genre, memoryStream, cancellationToken);
			var content = memoryStream.ToArray();

			await contentStorage.StoreContent(songFilePathParts, content, cancellationToken);

			// Enriching the song with content info
			song.Size = content.LongLength;
			song.Checksum = await checksumCalculator.CalculateChecksum(content, cancellationToken);
		}

		public async Task RollbackFolderCreation(Folder folder, CancellationToken cancellationToken)
		{
			var folderPathParts = await GetFolderPathParts(folder, cancellationToken);
			await contentStorage.DeleteEmptyFolder(folderPathParts, cancellationToken);
		}

		public async Task RollbackDiscCreation(Disc disc, CancellationToken cancellationToken)
		{
			var discFolderPathParts = (await GetDiscFolderPathParts(disc, cancellationToken))
				.ToList();

			if (disc.CoverFileName != null)
			{
				await contentStorage.DeleteContent(discFolderPathParts.Concat(new[] { disc.CoverFileName }), cancellationToken);
			}

			await contentStorage.DeleteEmptyFolder(discFolderPathParts, cancellationToken);
		}

		public async Task RollbackSongCreation(Song song, CancellationToken cancellationToken)
		{
			var songFilePathParts = await GetSongFilePathParts(song, cancellationToken);
			await contentStorage.DeleteContent(songFilePathParts, cancellationToken);
		}

		private async Task<IEnumerable<string>> GetFolderPathParts(Folder folder, CancellationToken cancellationToken)
		{
			if (folder.ParentFolderId == null)
			{
				throw new InvalidOperationException("Parent folder id is not set");
			}

			var parentFolderPathParts = await GetFolderPathParts(folder.ParentFolderId.Value, cancellationToken);
			return parentFolderPathParts.Concat(new[] { folder.Name });
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

		private async Task<IEnumerable<string>> GetDiscFolderPathParts(Disc disc, CancellationToken cancellationToken)
		{
			var parentFolderPathParts = await GetFolderPathParts(disc.FolderId, cancellationToken);
			return parentFolderPathParts.Concat(new[] { disc.TreeTitle });
		}

		private async Task<IEnumerable<string>> GetSongFilePathParts(Song song, CancellationToken cancellationToken)
		{
			var discsRepository = unitOfWork.DiscsRepository;
			var disc = await discsRepository.GetDisc(song.DiscId, cancellationToken);

			var discPathParts = await GetDiscFolderPathParts(disc, cancellationToken);
			return discPathParts.Concat(new[] { song.TreeTitle });
		}

		private static string GetCoverFileName(MemoryStream memoryStream)
		{
			ImageFormat imageFormat;

			try
			{
				using var image = Image.FromStream(memoryStream);
				imageFormat = image.RawFormat;
			}
			catch (ArgumentException e)
			{
				throw new ServiceOperationFailedException("Disc cover content is invalid", e);
			}

			if (imageFormat.Equals(ImageFormat.Jpeg))
			{
				return "cover.jpg";
			}

			if (imageFormat.Equals(ImageFormat.Png))
			{
				return "cover.png";
			}

			throw new ServiceOperationFailedException($"Image format '{imageFormat}' is not supported");
		}
	}
}
