using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class DiscOperations : BasicQuery, IDiscsQuery
	{
		public DiscOperations(ILogger<BasicQuery> logger, IOptions<ApiConnectionSettings> options)
			: base(logger, options)
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
