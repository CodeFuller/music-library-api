using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Logic.Extensions;
using MusicLibraryApi.Logic.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Services
{
	public class FoldersService : IFoldersService
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly IFoldersRepository repository;

		private readonly IStorageService storageService;

		private readonly ILogger<FoldersService> logger;

		public FoldersService(IUnitOfWork unitOfWork, IStorageService storageService, ILogger<FoldersService> logger)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.repository = unitOfWork.FoldersRepository;
		}

		public async Task<int> CreateFolder(Folder folder, CancellationToken cancellationToken)
		{
			try
			{
				// Creating folder in the storage.
				await storageService.CreateFolder(folder, cancellationToken);
			}
			catch (FolderNotFoundException e)
			{
				throw e.Handle(folder.ParentFolderId, logger);
			}

			try
			{
				// Creating folder in the repository.
				await repository.AddFolder(folder, cancellationToken);
				await unitOfWork.Commit(cancellationToken);

				return folder.Id;
			}
			catch (Exception e)
			{
				await RollbackFolderCreation(folder, cancellationToken);

				if (e is DuplicateKeyException)
				{
					logger.LogError(e, "Folder {FolderName} already exists", folder.Name);
					throw new ServiceOperationFailedException(Invariant($"Folder '{folder.Name}' already exists"), e);
				}

				throw;
			}
		}

		private async Task RollbackFolderCreation(Folder folder, CancellationToken cancellationToken)
		{
			try
			{
				await storageService.RollbackFolderCreation(folder, cancellationToken);
			}
#pragma warning disable CA1031 // Do not catch general exception types - All exceptions are caught for rollback to throw initial exception thrown by Commit().
			catch (Exception rollbackException)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				logger.LogError(rollbackException, "Failed to rollback folder {FolderName} created in the storage", folder.Name);
			}
		}

		public async Task<IDictionary<int, Folder>> GetFolders(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			var folders = await repository.GetFolders(folderIds, cancellationToken);
			return folders.ToDictionary(f => f.Id);
		}

		public async Task<Folder> GetFolder(int? folderId, CancellationToken cancellationToken)
		{
			var folderIdValue = folderId ?? await repository.GetRooFolderId(cancellationToken);

			try
			{
				return await repository.GetFolder(folderIdValue, cancellationToken);
			}
			catch (FolderNotFoundException e)
			{
				throw e.Handle(folderIdValue, logger);
			}
		}

		public async Task<ILookup<int, Folder>> GetSubfoldersByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			var subfolders = await repository.GetSubfoldersByFolderIds(folderIds, cancellationToken);
			return subfolders
				.OrderBy(f => f.Name)
				.ToLookup(f => f.ParentFolderId ?? 0);
		}
	}
}
