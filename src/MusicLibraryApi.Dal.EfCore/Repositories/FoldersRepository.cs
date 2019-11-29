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
				folderEntity.ParentFolder = await FindFolder(parentFolderId.Value, includeChildFolders: false, includeDiscs: false, cancellationToken);
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

		public async Task<IReadOnlyCollection<Folder>> GetSubfolders(int? folderId, CancellationToken cancellationToken)
		{
			// The logic differs for non-root and root folders.
			// In first case we should verify that passed folder exist.
			// For root folder, when null is passed, no verification is required.
			// Second reason for logic difference: we don't have synthetic root folder entity,
			// which we could fetch and call ChildFolders on it.
			if (folderId != null)
			{
				var parentFolder = await FindFolder(folderId.Value, includeChildFolders: true, includeDiscs: false, cancellationToken);
				return parentFolder.ChildFolders.Select(e => mapper.Map<Folder>(e)).ToList();
			}

			return await context.Folders
				.Where(f => f.ParentFolder == null)
				.Select(f => mapper.Map<Folder>(f))
				.ToListAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<Disc>> GetFolderDiscs(int? folderId, CancellationToken cancellationToken)
		{
			if (folderId != null)
			{
				var folder = await FindFolder(folderId.Value, includeChildFolders: false, includeDiscs: true, cancellationToken);
				return folder.Discs.Select(e => mapper.Map<Disc>(e)).ToList();
			}

			return await context.Discs
				.Where(d => d.Folder == null)
				.Select(d => mapper.Map<Disc>(d))
				.ToListAsync(cancellationToken);
		}

		private async Task<FolderEntity> FindFolder(int folderId, bool includeChildFolders, bool includeDiscs, CancellationToken cancellationToken)
		{
			IQueryable<FolderEntity> query = context.Folders
				.Include(f => f.ParentFolder);

			if (includeChildFolders)
			{
				query = query.Include(f => f.ChildFolders);
			}

			if (includeDiscs)
			{
				query = query.Include(f => f.Discs);
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
