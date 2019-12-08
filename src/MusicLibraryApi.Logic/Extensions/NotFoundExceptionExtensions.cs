using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using static System.FormattableString;

namespace MusicLibraryApi.Logic.Extensions
{
	public static class NotFoundExceptionExtensions
	{
		public static ServiceOperationFailedException Handle(this FolderNotFoundException e, int folderId, ILogger logger)
		{
			logger.LogError(e, "The folder with id of {FolderId} does not exist", folderId);
			return new ServiceOperationFailedException(Invariant($"The folder with id of '{folderId}' does not exist"), e);
		}

		public static ServiceOperationFailedException Handle(this DiscNotFoundException e, int discId, ILogger logger)
		{
			logger.LogError(e, "The disc with id of {DiscId} does not exist", discId);
			throw new ServiceOperationFailedException(Invariant($"The disc with id of '{discId}' does not exist"), e);
		}

		public static ServiceOperationFailedException Handle(this ArtistNotFoundException e, int? artistId, ILogger logger)
		{
			logger.LogError(e, "The artist with id of {ArtistId} does not exist", artistId);
			throw new ServiceOperationFailedException(Invariant($"The artist with id of '{artistId}' does not exist"), e);
		}

		public static ServiceOperationFailedException Handle(this GenreNotFoundException e, int? genreId, ILogger logger)
		{
			logger.LogError(e, "The genre with id of {GenreId} does not exist", genreId);
			throw new ServiceOperationFailedException(Invariant($"The genre with id of '{genreId}' does not exist"), e);
		}
	}
}
