using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	internal class ArtistOperations : BasicQuery, IArtistsQuery, IArtistsMutation
	{
		public ArtistOperations(IHttpClientFactory httpClientFactory, ILogger<ArtistOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputArtistData>> GetArtists(QueryFieldSet<OutputArtistData> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<OutputArtistData, OutputArtistData[]>("artists", fields, cancellationToken);
		}

		public async Task<OutputArtistData> GetArtist(int artistId, QueryFieldSet<OutputArtistData> fields, CancellationToken cancellationToken)
		{
			var query = Invariant($@"query GetArtistById($id: ID!) {{
										artist(id: $id) {{ {fields.QuerySelection} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					id = artistId,
				},
			};

			return await ExecuteRequest<OutputArtistData>("artist", request, cancellationToken);
		}

		public async Task<int> CreateArtist(InputArtistData artistData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new artist {ArtistName} ...", artistData.Name);

			var query = Invariant($@"mutation ($artist: ArtistInput!) {{
										createArtist(artist: $artist) {{ newArtistId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					artist = artistData,
				},
			};

			var data = await ExecuteRequest<CreateArtistOutputData>("createArtist", request, cancellationToken);

			var newArtistId = data.NewArtistId;

			if (newArtistId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created artist {artistData.Name}");
			}

			Logger.LogInformation("Created new artist {ArtistName} with id of {ArtistId}", artistData.Name, newArtistId);

			return newArtistId.Value;
		}
	}
}
