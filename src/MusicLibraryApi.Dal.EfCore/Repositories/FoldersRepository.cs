using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Dal.EfCore.Entities;
using static System.FormattableString;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class FoldersRepository : IFoldersRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public static int RootFolderId => 1;

		public FoldersRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public Task<int> GetRooFolderId(CancellationToken cancellationToken)
		{
			return Task.FromResult(RootFolderId);
		}

		public async Task<int> CreateFolder(Folder folder, CancellationToken cancellationToken)
		{
			if (folder.ParentFolderId == null)
			{
				throw new InvalidOperationException("Can not create a folder without a parent");
			}

			var folderEntity = mapper.Map<FolderEntity>(folder);
			context.Folders.Add(folderEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException())
			{
				throw new FolderNotFoundException(Invariant($"The parent folder with id of {folder.ParentFolderId} does not exist"));
			}
			catch (DbUpdateException e) when (e.IsUniqueViolationException())
			{
				throw new DuplicateKeyException($"Failed to add folder '{folder.Name}' to the database", e);
			}

			return folderEntity.Id;
		}

		public async Task<IReadOnlyCollection<Folder>> GetFolders(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			return await context.Folders.Where(f => folderIds.Contains(f.Id))
				.Select(f => mapper.Map<Folder>(f))
				.ToListAsync(cancellationToken);
		}

		public async Task<Folder> GetFolder(int folderId, CancellationToken cancellationToken)
		{
			var folderEntity = await context.Folders
				.Where(f => f.Id == folderId)
				.SingleOrDefaultAsync(cancellationToken);

			if (folderEntity == null)
			{
				throw new FolderNotFoundException(Invariant($"The folder with id of {folderId} does not exist"));
			}

			return mapper.Map<Folder>(folderEntity);
		}

		public async Task<IReadOnlyCollection<Folder>> GetSubfoldersByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			return await context.Folders.Where(f => f.ParentFolderId != null && folderIds.Contains(f.ParentFolderId.Value))
				.Select(f => mapper.Map<Folder>(f))
				.ToListAsync(cancellationToken);
		}
	}
}
