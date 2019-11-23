using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class DiscOperations : BasicQuery, IDiscsQuery
	{
		public DiscOperations(IHttpClientFactory httpClientFactory, ILogger<BasicQuery> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async IAsyncEnumerable<OutputDiscData> GetDiscs(QueryFieldSet fields, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			var discs = await ExecuteQuery<OutputDiscData[]>("discs", fields, cancellationToken);

			foreach (var disc in discs)
			{
				yield return disc;
			}
		}

		public async Task<OutputDiscData> GetDisc(int discId, QueryFieldSet fields, CancellationToken cancellationToken)
		{
			var requestedFields = JoinRequestFields(fields);

			var query = Invariant($@"query GetDiscById($id: ID!) {{
										disc(id: $id) {{ {requestedFields} }}
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
	}
}
