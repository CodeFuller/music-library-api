using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class FolderOperations : BasicQuery, IFoldersQuery, IFoldersMutation
	{
		public FolderOperations(IHttpClientFactory httpClientFactory, ILogger<FolderOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputFolderData>> GetSubfolders(int? folderId, QueryFieldSet<FolderQuery> fields, CancellationToken cancellationToken)
		{
			var requestedFields = JoinRequestFields(fields);

			var query = Invariant($@"query GetFolderSubfolders($folderId: ID) {{
										folderSubfolders(folderId: $folderId) {{ {requestedFields} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					folderId = folderId,
				},
			};

			return await ExecuteRequest<OutputFolderData[]>("folderSubfolders", request, cancellationToken);
		}

		public async Task<IReadOnlyCollection<OutputDiscData>> GetFolderDiscs(int? folderId, QueryFieldSet<DiscQuery> fields, CancellationToken cancellationToken)
		{
			var requestedFields = JoinRequestFields(fields);

			var query = Invariant($@"query GetFolderDiscs($folderId: ID) {{
										folderDiscs(folderId: $folderId) {{ {requestedFields} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					folderId = folderId,
				},
			};

			return await ExecuteRequest<OutputDiscData[]>("folderDiscs", request, cancellationToken);
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
