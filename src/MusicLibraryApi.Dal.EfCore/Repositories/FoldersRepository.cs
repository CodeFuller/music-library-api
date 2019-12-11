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

		public async Task<int> CreateFolder(int parentFolderId, Folder folder, CancellationToken cancellationToken)
		{
			var folderEntity = mapper.Map<FolderEntity>(folder);
			folderEntity.ParentFolder = await context.FindFolder(parentFolderId, cancellationToken);

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

		public async Task<Folder> GetFolder(int folderId, CancellationToken cancellationToken)
		{
			var folder = await context.FindFolder(folderId, cancellationToken);
			return mapper.Map<Folder>(folder);
		}

		public async Task<IReadOnlyCollection<Folder>> GetSubfoldersByFolderIds(IEnumerable<int> folderIds, CancellationToken cancellationToken)
		{
			return await context.Folders.Where(f => f.ParentFolder != null && folderIds.Contains(f.ParentFolder.Id))
				.Select(f => mapper.Map<Folder>(f))
				.ToListAsync(cancellationToken);
		}
	}
}
