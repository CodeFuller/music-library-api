using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Logic.Services
{
	public class FoldersService : IFoldersService
	{
		private readonly IFoldersRepository repository;

		public FoldersService(IFoldersRepository repository)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<int> CreateFolder(int? parentFolderId, string folderName, CancellationToken cancellationToken)
		{
			var folder = new Folder(folderName, null);

			return await repository.CreateFolder(parentFolderId, folder, cancellationToken);
		}

		public async Task<IReadOnlyCollection<Folder>> GetSubfolders(int? parentFolderId, CancellationToken cancellationToken)
		{
			var subfolders = await repository.GetSubfolders(parentFolderId, cancellationToken);

			return subfolders.OrderBy(f => f.Name).ToList();
		}
	}
}
