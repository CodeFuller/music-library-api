using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Logic.Interfaces;
using MusicLibraryApi.Logic.Settings;

namespace MusicLibraryApi.Logic.Internal
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class FileSystemContentStorage : IContentStorage
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		private readonly string rootPath;

		public FileSystemContentStorage(IOptions<FileSystemStorageSettings> options)
		{
			var settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
			if (String.IsNullOrWhiteSpace(settings.Root))
			{
				throw new InvalidOperationException("The root path of file system storage is not set");
			}

			if (!Directory.Exists(settings.Root))
			{
				throw new InvalidOperationException($"The root path of file system storage does not exist: '{settings.Root}'");
			}

			rootPath = settings.Root;
		}

		public Task CreateFolder(string path, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(path);
			Directory.CreateDirectory(fullPath);

			return Task.CompletedTask;
		}

		public Task DeleteEmptyFolder(string path, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(path);
			Directory.Delete(fullPath);

			return Task.CompletedTask;
		}

		public async Task StoreContent(string path, byte[] content, CancellationToken cancellationToken)
		{
			var fullPath = GetFullPath(path);

			await using var fileStream = File.Open(fullPath, FileMode.CreateNew);
			await using var contentStream = new MemoryStream(content);
			await contentStream.CopyToAsync(fileStream, cancellationToken);

			File.SetAttributes(fullPath, FileAttributes.ReadOnly);
		}

		private string GetFullPath(string path)
		{
			return Path.Combine(rootPath, path);
		}
	}
}
