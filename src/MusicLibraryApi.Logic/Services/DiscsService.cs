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
using MusicLibraryApi.Logic.Interfaces;

namespace MusicLibraryApi.Logic.Services
{
	public class DiscsService : IDiscsService
	{
		private readonly IUnitOfWork unitOfWork;

		private readonly IDiscsRepository repository;

		private readonly IStorageService storageService;

		private readonly ILogger<DiscsService> logger;

		public DiscsService(IUnitOfWork unitOfWork, IStorageService storageService, ILogger<DiscsService> logger)
		{
			this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.repository = unitOfWork.DiscsRepository;
		}

		public async Task<int> CreateDisc(Disc disc, CancellationToken cancellationToken)
		{
			try
			{
				// Creating disc in the storage.
				await storageService.CreateDisc(disc, cancellationToken);

				// Creating disc in the repository.
				await repository.AddDisc(disc, cancellationToken);
				await unitOfWork.Commit(cancellationToken);

				return disc.Id;
			}
			catch (FolderNotFoundException e)
			{
				throw e.Handle(disc.FolderId, logger);
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

		public async Task<IDictionary<int, Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken)
		{
			var discs = await repository.GetDiscs(discIds, cancellationToken);
			return discs.ToDictionary(d => d.Id);
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
				.ToLookup(d => d.FolderId);
		}
	}
}
