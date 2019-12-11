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

		public async Task<int> CreateFolder(int parentFolderId, string folderName, CancellationToken cancellationToken)
		{
			try
			{
				var folder = new Folder(folderName, parentFolderId);
				return await repository.CreateFolder(folder, cancellationToken);
			}
			catch (DuplicateKeyException e)
			{
				logger.LogError(e, "Folder {FolderName} already exists", folderName);
				throw new ServiceOperationFailedException(Invariant($"Folder '{folderName}' already exists"), e);
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
