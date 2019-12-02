using System;
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
				folderEntity.ParentFolder = await context.FindFolder(parentFolderId.Value, includeSubfolders: false, includeDiscs: false, cancellationToken);
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

		public async Task<Folder> GetFolder(int? folderId, bool loadSubfolders, bool loadDiscs, CancellationToken cancellationToken)
		{
			if (folderId == null)
			{
				var subfolders = loadSubfolders ? await context.Folders
					.Where(f => f.ParentFolder == null)
					.Select(f => mapper.Map<Folder>(f))
					.ToListAsync(cancellationToken) : null;

				var discs = loadDiscs ? await context.Discs
					.Where(d => d.Folder == null)
					.Select(d => mapper.Map<Disc>(d))
					.ToListAsync(cancellationToken) : null;

				return new Folder("<ROOT>", subfolders, discs);
			}

			var folder = await context.FindFolder(folderId.Value, loadSubfolders, loadDiscs, cancellationToken);
			return mapper.Map<Folder>(folder);
		}
	}
}
