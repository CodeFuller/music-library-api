using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Services
{
	public class FoldersService : IFoldersService
	{
		private readonly IFoldersRepository repository;

		private readonly ILogger<FoldersService> logger;

		public FoldersService(IFoldersRepository repository, ILogger<FoldersService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> CreateFolder(int? parentFolderId, string folderName, CancellationToken cancellationToken)
		{
			try
			{
				var folder = new Folder(folderName, null);
				return await repository.CreateFolder(parentFolderId, folder, cancellationToken);
			}
			catch (DuplicateKeyException e)
			{
				logger.LogError(e, "Folder {FolderName} already exists", folderName);
				throw new ServiceOperationFailedException(Invariant($"Folder '{folderName}' already exists"), e);
			}
		}

		public async Task<IReadOnlyCollection<Folder>> GetSubfolders(int? folderId, CancellationToken cancellationToken)
		{
			try
			{
				var subfolders = await repository.GetSubfolders(folderId, cancellationToken);
				return subfolders.OrderBy(f => f.Name).ToList();
			}
			catch (NotFoundException e)
			{
				logger.LogError(e, "The folder with id of {FolderId} does not exist", folderId);
				throw new ServiceOperationFailedException(Invariant($"The folder with id of '{folderId}' does not exist"), e);
			}
		}

		public async Task<IReadOnlyCollection<Disc>> GetFolderDiscs(int? folderId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetFolderDiscs(folderId, cancellationToken);
			}
			catch (NotFoundException e)
			{
				logger.LogError(e, "The folder with id of {FolderId} does not exist", folderId);
				throw new ServiceOperationFailedException(Invariant($"The folder with id of '{folderId}' does not exist"), e);
			}
		}
	}
}
