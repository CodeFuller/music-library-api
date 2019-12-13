using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Dal.EfCore.Repositories;

namespace MusicLibraryApi.Dal.EfCore
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly MusicLibraryDbContext context;

		private readonly Dictionary<string, Func<string, Exception, NotFoundException>> constraintNameToExceptionMapping;

		public IFoldersRepository FoldersRepository { get; }

		public IDiscsRepository DiscsRepository { get; }

		public IArtistsRepository ArtistsRepository { get; }

		public IGenresRepository GenresRepository { get; }

		public ISongsRepository SongsRepository { get; }

		public IPlaybacksRepository PlaybacksRepository { get; }

		public UnitOfWork(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));

			FoldersRepository = new FoldersRepository(context);
			DiscsRepository = new DiscsRepository(context);
			ArtistsRepository = new ArtistsRepository(context);
			GenresRepository = new GenresRepository(context);
			SongsRepository = new SongsRepository(context);
			PlaybacksRepository = new PlaybacksRepository(context);

			constraintNameToExceptionMapping = new Dictionary<string, Func<string, Exception, NotFoundException>>
			{
				[MusicLibraryDbContext.DiscFolderForeignKeyName] = (m, e) => new FolderNotFoundException(m, e),
				[MusicLibraryDbContext.FolderParentFolderForeignKeyName] = (m, e) => new FolderNotFoundException(m, e),
				[MusicLibraryDbContext.SongDiscForeignKeyName] = (m, e) => new DiscNotFoundException(m, e),
				[MusicLibraryDbContext.SongArtistForeignKeyName] = (m, e) => new ArtistNotFoundException(m, e),
				[MusicLibraryDbContext.SongGenreForeignKeyName] = (m, e) => new GenreNotFoundException(m, e),
			};
		}

		public async Task Commit(CancellationToken cancellationToken)
		{
			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsUniqueViolationException())
			{
				throw new DuplicateKeyException("Failed to save DB changes", e);
			}
			catch (DbUpdateException e) when (e.IsForeignKeyViolationException(out var constraintName) &&
			                                  constraintNameToExceptionMapping.TryGetValue(constraintName ?? String.Empty, out var exceptionFactory))
			{
				throw exceptionFactory("Failed to save DB changes", e);
			}
		}
	}
}
