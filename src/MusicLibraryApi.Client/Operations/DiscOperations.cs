using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class DiscOperations : BasicQuery, IDiscsQuery, IDiscsMutation
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public DiscOperations(IHttpClientFactory httpClientFactory, ILogger<DiscOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputDiscData>> GetDiscs(QueryFieldSet<OutputDiscData> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<OutputDiscData, OutputDiscData[]>("discs", fields, cancellationToken);
		}

		public async Task<OutputDiscData> GetDisc(int discId, QueryFieldSet<OutputDiscData> fields, CancellationToken cancellationToken)
		{
			var query = Invariant($@"query GetDiscById($id: ID!) {{
										disc(id: $id) {{ {fields.QuerySelection} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					id = discId,
				},
			};

			return await ExecuteRequest<OutputDiscData>("disc", request, cancellationToken);
		}

		public async Task<int> CreateDisc(InputDiscData discData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new disc {DiscTitle} ...", discData.Title);

			var query = Invariant($@"mutation ($disc: DiscInput!) {{
										createDisc(disc: $disc) {{ newDiscId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					disc = discData,
				},
			};

			var data = await ExecuteRequest<CreateDiscOutputData>("createDisc", request, cancellationToken);

			var newDiscId = data.NewDiscId;

			if (newDiscId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created disc {discData.Title}");
			}

			Logger.LogInformation("Created new disc {DiscTitle} with id of {DiscId}", discData.Title, newDiscId);

			return newDiscId.Value;
		}
	}
}
