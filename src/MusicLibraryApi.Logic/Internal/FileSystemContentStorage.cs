using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Logic.Interfaces;
using MusicLibraryApi.Logic.Settings;

namespace MusicLibraryApi.Logic.Internal
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class FileSystemContentStorage : IContentStorage
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		private readonly IFileSystemFacade fileSystemFacade;

		private readonly string rootPath;

		public FileSystemContentStorage(IFileSystemFacade fileSystemFacade, IOptions<FileSystemStorageSettings> options)
		{
			this.fileSystemFacade = fileSystemFacade ?? throw new ArgumentNullException(nameof(fileSystemFacade));

			var settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
			if (String.IsNullOrWhiteSpace(settings.Root))
			{
				throw new InvalidOperationException("The root path of file system storage is not set");
			}

			if (!fileSystemFacade.DirectoryExists(settings.Root))
			{
				throw new InvalidOperationException($"The root path of file system storage does not exist: '{settings.Root}'");
			}

			rootPath = settings.Root;
		}

		public Task CreateFolder(IEnumerable<string> pathParts, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(pathParts);
			fileSystemFacade.CreateDirectory(fullPath);

			return Task.CompletedTask;
		}

		public Task DeleteEmptyFolder(IEnumerable<string> pathParts, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(pathParts);
			fileSystemFacade.DeleteDirectory(fullPath);

			return Task.CompletedTask;
		}

		public async Task StoreContent(IEnumerable<string> pathParts, byte[] content, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(pathParts);

			await using var fileStream = fileSystemFacade.OpenFile(fullPath, FileMode.CreateNew);
			await using var contentStream = new MemoryStream(content);
			await contentStream.CopyToAsync(fileStream, cancellationToken);

			fileSystemFacade.SetReadOnlyAttribute(fullPath);
		}

		public Task DeleteContent(IEnumerable<string> pathParts, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(pathParts);

			fileSystemFacade.ClearReadOnlyAttribute(fullPath);
			fileSystemFacade.DeleteFile(fullPath);

			return Task.CompletedTask;
		}

		private string GetFullPath(IEnumerable<string> pathParts)
		{
			var relativePath = CombinePathParts(pathParts);
			return Path.Combine(rootPath, relativePath);
		}

		private static string CombinePathParts(IEnumerable<string> parts)
		{
			// We skip the first root folder.
			return parts
				.Skip(1)
				.Select(SafePathName)
				.Aggregate((currPath, currPart) => currPath.Length == 0 ? currPart : Path.Combine(currPath, currPart));
		}

		private static string SafePathName(string name)
		{
			var invalidFileNameChars = Path.GetInvalidFileNameChars();

			var parts = name.Select(c => invalidFileNameChars.Contains(c) ? ReplaceInvalidFileNameChar(c) : new String(c, 1));
			var resultName = String.Join(String.Empty, parts);

			if (resultName.Length == 0)
			{
				throw new ServiceOperationFailedException($"Can not build safe file name for '{name}'");
			}

			return resultName;
		}

		private static string ReplaceInvalidFileNameChar(char c)
		{
			var charactersReplacements = new Dictionary<char, string>
			{
				{ '?', String.Empty },
				{ '"', "'" },
				{ ':', "-" },
				{ '/', "-" },
				{ '|', "-" },
			};

			return charactersReplacements.TryGetValue(c, out var replaced) ? replaced : String.Empty;
		}
	}
}
