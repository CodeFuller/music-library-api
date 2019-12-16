using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class FolderOperations : BasicQuery, IFoldersQuery, IFoldersMutation
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public FolderOperations(IHttpClientFactory httpClientFactory, ILogger<FolderOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<OutputFolderData> GetFolder(int? folderId, QueryFieldSet<OutputFolderData> fields, CancellationToken cancellationToken, bool includeDeletedDiscs = false)
		{
			var query = Invariant($@"query GetFolder($folderId: ID, {fields.VariablesDefinition}) {{
										folder(folderId: $folderId) {{ {fields.QuerySelection} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					folderId = folderId,
					includeDeletedDiscs = includeDeletedDiscs,
				},
			};

			return await ExecuteRequest<OutputFolderData>("folder", request, cancellationToken);
		}

		public async Task<int> CreateFolder(InputFolderData folderData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new folder {FolderName} within parent folder {ParentFolderId} ...", folderData.Name, folderData.ParentFolderId);

			var query = Invariant($@"mutation ($folder: FolderInput!) {{
										createFolder(folder: $folder) {{ newFolderId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					folder = folderData,
				},
			};

			var data = await ExecuteRequest<CreateFolderOutputData>("createFolder", request, cancellationToken);

			var newFolderId = data.NewFolderId;

			if (newFolderId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created folder {folderData.Name}");
			}

			Logger.LogInformation("Created new folder {FolderName} with id of {FolderId}", folderData.Name, newFolderId);

			return newFolderId.Value;
		}
	}
}
