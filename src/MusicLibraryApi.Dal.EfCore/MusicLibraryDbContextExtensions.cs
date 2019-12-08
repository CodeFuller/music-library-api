﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Dal.EfCore.Entities;
using static System.FormattableString;

namespace MusicLibraryApi.Dal.EfCore
{
	public static class MusicLibraryDbContextExtensions
	{
		public static async Task<FolderEntity> FindFolder(this MusicLibraryDbContext context, int folderId, CancellationToken cancellationToken)
		{
			var folderEntity = await context.Folders
				.Include(f => f.ParentFolder)
				.Where(x => x.Id == folderId)
				.SingleOrDefaultAsync(cancellationToken);

			if (folderEntity == null)
			{
				throw new FolderNotFoundException(Invariant($"The folder with id of {folderId} does not exist"));
			}

			return folderEntity;
		}
	}
}
