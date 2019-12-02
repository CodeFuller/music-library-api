using System.Linq;
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
		public static async Task<FolderEntity> FindFolder(this MusicLibraryDbContext context, int folderId, bool includeSubfolders, bool includeDiscs, CancellationToken cancellationToken)
		{
			IQueryable<FolderEntity> query = context.Folders
				.Include(f => f.ParentFolder);

			if (includeSubfolders)
			{
				query = query.Include(f => f.Subfolders);
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
