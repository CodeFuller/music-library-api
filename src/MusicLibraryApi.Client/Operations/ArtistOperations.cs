﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class ArtistOperations : BasicQuery, IArtistsQuery, IArtistsMutation
	{
		public ArtistOperations(IHttpClientFactory httpClientFactory, ILogger<ArtistOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputArtistData>> GetArtists(QueryFieldSet<ArtistQuery> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<ArtistQuery, OutputArtistData[]>("artists", fields, cancellationToken);
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
