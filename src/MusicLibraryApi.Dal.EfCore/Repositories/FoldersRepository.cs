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

		public FoldersRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreateFolder(int? parentFolderId, Folder folder, CancellationToken cancellationToken)
		{
			var folderEntity = mapper.Map<FolderEntity>(folder);

			if (parentFolderId != null)
			{
				folderEntity.ParentFolder = await FindFolder(parentFolderId.Value, false, cancellationToken);
			}

			context.Folders.Add(folderEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsUniqueViolationException())
			{
				throw new DuplicateKeyException($"Failed to add folder '{folder.Name}' to the database", e);
			}

			return folderEntity.Id;
		}

		public async Task<IReadOnlyCollection<Folder>> GetSubfolders(int? parentFolderId, CancellationToken cancellationToken)
		{
			// The logic differs for non-root and root folders.
			// In first case we should verify that passed folder exist.
			// For root folder, when null is passed, no verification is required.
			// Second reason for logic difference: we don't have synthetic root folder entity,
			// which we could fetch and call ChildFolders on it.
			if (parentFolderId != null)
			{
				var parentFolder = await FindFolder(parentFolderId.Value, true, cancellationToken);
				return parentFolder.ChildFolders.Select(e => mapper.Map<Folder>(e)).ToList();
			}

			return await context.Folders
				.Where(f => (f.ParentFolder != null ? (int?)f.ParentFolder.Id : null) == parentFolderId)
				.OrderBy(f => f.Name)
				.Select(f => mapper.Map<Folder>(f))
				.ToListAsync(cancellationToken);
		}

		private async Task<FolderEntity> FindFolder(int folderId, bool includeChildFolders, CancellationToken cancellationToken)
		{
			IQueryable<FolderEntity> query = context.Folders
				.Include(f => f.ParentFolder);

			if (includeChildFolders)
			{
				query = query.Include(f => f.ChildFolders);
			}

			var folderEntity = await query
				.Where(x => x.Id == folderId)
				.SingleOrDefaultAsync(cancellationToken);

			if (folderEntity == null)
			{
				throw new NotFoundException(Invariant($"The folder with id of {folderId} does not exist"));
			}

			return folderEntity;
		}
	}
}
