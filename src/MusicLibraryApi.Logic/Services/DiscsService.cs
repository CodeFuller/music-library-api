using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using static System.FormattableString;

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
			catch (NotFoundException e)
			{
				logger.LogError(e, "The folder with id of {FolderId} does not exist", folderId);
				throw new ServiceOperationFailedException(Invariant($"The folder with id of '{folderId}' does not exist"), e);
			}
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			try
			{
				return await repository.GetDisc(discId, cancellationToken);
			}
			catch (NotFoundException e)
			{
				logger.LogError(e, "The disc with id of {DiscId} does not exist", discId);
				throw new ServiceOperationFailedException(Invariant($"The disc with id of '{discId}' does not exist"), e);
			}
		}

		public Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return repository.GetAllDiscs(cancellationToken);
		}
	}
}
