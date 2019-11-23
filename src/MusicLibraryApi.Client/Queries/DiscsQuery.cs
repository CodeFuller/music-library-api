using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Client.Contracts.Output;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Queries
{
	public class DiscsQuery : BasicQuery, IDiscsQuery
	{
		public DiscsQuery(ILogger<BasicQuery> logger, IOptions<ApiConnectionSettings> options)
			: base(logger, options)
		{
		}

		public async IAsyncEnumerable<DiscDto> GetDiscs(QueryFieldSet fields, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			var discs = await ExecuteQuery<DiscDto[]>("discs", fields, cancellationToken);

			foreach (var disc in discs)
			{
				yield return disc;
			}
		}

		public async Task<DiscDto> GetDisc(int discId, QueryFieldSet fields, CancellationToken cancellationToken)
		{
			var requestedFields = String.Join(" ", fields.Select(f => f.Name));

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

			return await ExecuteRequest<DiscDto>("disc", request, cancellationToken);
		}
	}
}
