using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Extensions;
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.Logic.Services
{
	public class DiscsService : IDiscsService
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly IDiscsRepository repository;

		private readonly IStorageService storageService;

		private readonly ILogger<DiscsService> logger;

		public DiscsService(IUnitOfWork unitOfWork, IStorageService storageService, ILogger<DiscsService> logger)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.repository = unitOfWork.DiscsRepository;
		}

		public Task<int> CreateDisc(Disc disc, CancellationToken cancellationToken)
		{
			return CreateDiscInternal(disc, null, cancellationToken);
		}

		public Task<int> CreateDisc(Disc disc, Stream coverContent, CancellationToken cancellationToken)
		{
			_ = coverContent ?? throw new ArgumentNullException(nameof(coverContent));

			return CreateDiscInternal(disc, coverContent, cancellationToken);
		}

		private async Task<int> CreateDiscInternal(Disc disc, Stream? coverContent, CancellationToken cancellationToken)
		{
			await CreateDiscInStorage(disc, coverContent, cancellationToken);

			return await CreateDiscInRepository(disc, cancellationToken);
		}

		private async Task CreateDiscInStorage(Disc disc, Stream? coverContent, CancellationToken cancellationToken)
		{
			if (!disc.IsDeleted)
			{
				try
				{
					// Creating disc in the storage.
					await storageService.CreateDisc(disc, cancellationToken);
				}
				catch (FolderNotFoundException e)
				{
					throw e.Handle(disc.FolderId, logger);
				}

				if (coverContent != null)
				{
					await storageService.StoreDiscCover(disc, coverContent, cancellationToken);
				}
			}
			else
			{
				if (coverContent != null)
				{
					throw new InvalidOperationException("Can not create deleted disc with cover");
				}
			}
		}

		private async Task<int> CreateDiscInRepository(Disc disc, CancellationToken cancellationToken)
		{
			try
			{
				// Creating disc in the repository.
				await repository.AddDisc(disc, cancellationToken);
				await unitOfWork.Commit(cancellationToken);

				return disc.Id;
			}
			catch (Exception e)
			{
				await RollbackDiscCreation(disc, cancellationToken);

#pragma warning disable CA1508 // Avoid dead conditional code - False positive
				if (e is FolderNotFoundException folderNotFoundException)
#pragma warning restore CA1508 // Avoid dead conditional code
				{
					throw folderNotFoundException.Handle(disc.FolderId, logger);
				}

				throw;
			}
		}

		private async Task RollbackDiscCreation(Disc disc, CancellationToken cancellationToken)
		{
			try
			{
				await storageService.RollbackDiscCreation(disc, cancellationToken);
			}
#pragma warning disable CA1031 // Do not catch general exception types - All exceptions are caught for rollback to throw initial exception thrown by Commit().
			catch (Exception rollbackException)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				logger.LogError(rollbackException, "Failed to rollback disc {DiscTitle} created in the storage", disc.Title);
			}
		}

		public async Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			var discs = await repository.GetAllDiscs(cancellationToken);

			// There is no meaningful sorting for all discs. We sort them by id here mostly for steady IT baselines.
			return discs
				.Where(d => !d.IsDeleted)
				.OrderBy(d => d.Id).ToList();
		}

		public async Task<IDictionary<int, Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			var discs = await repository.GetDiscs(discIds, cancellationToken);
			return discs.ToDictionary(d => d.Id);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetDisc(discId, cancellationToken);
			}
			catch (DiscNotFoundException e)
			{
				throw e.Handle(discId, logger);
			}
		}

		public async Task<ILookup<int, Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, bool includeDeletedDiscs, CancellationToken cancellationToken)
		{
			var discs = await repository.GetDiscsByFolderIds(folderIds, cancellationToken);
			return discs
				.Where(d => includeDeletedDiscs || !d.IsDeleted)
				.OrderBy(d => d.Year == null)
				.ThenBy(d => d.Year)
				.ThenBy(d => d.AlbumTitle)
				.ThenBy(d => d.AlbumOrder)
				.ToLookup(d => d.FolderId);
		}
	}
}
