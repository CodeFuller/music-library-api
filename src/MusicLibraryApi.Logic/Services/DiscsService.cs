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

namespace MusicLibraryApi.Logic.Services
{
	public class DiscsService : IDiscsService
	{
		private readonly IDiscsRepository repository;

		private readonly ILogger<DiscsService> logger;

		public DiscsService(IDiscsRepository repository, ILogger<DiscsService> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> CreateDisc(int folderId, Disc disc, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.CreateDisc(folderId, disc, cancellationToken);
			}
			catch (FolderNotFoundException e)
			{
				throw e.Handle(folderId, logger);
			}
		}

		public async Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			var discs = await repository.GetAllDiscs(cancellationToken);

			// There is no meaningful sorting for all discs. We sort them by id here mostly for steady IT baselines.
			return discs
				.Where(d => !d.IsDeleted)
				.OrderBy(d => d.Id).ToList();
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetDisc(discId, cancellationToken);
			}
			catch (DiscNotFoundException e)
			{
				throw e.Handle(discId, logger);
			}
		}

		public async Task<ILookup<int, Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, bool includeDeletedDiscs, CancellationToken cancellationToken)
		{
			var discs = await repository.GetDiscsByFolderIds(folderIds, cancellationToken);
			return discs
				.Where(d => includeDeletedDiscs || !d.IsDeleted)
				.OrderBy(d => d.Year == null)
				.ThenBy(d => d.Year)
				.ThenBy(d => d.AlbumTitle)
				.ThenBy(d => d.AlbumOrder)
				.ToLookup(d => d.Folder.Id);
		}
	}
}
