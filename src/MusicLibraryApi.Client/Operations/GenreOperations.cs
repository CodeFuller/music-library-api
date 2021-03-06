﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	internal class GenreOperations : BasicQuery, IGenresQuery, IGenresMutation
	{
		public GenreOperations(IHttpClientFactory httpClientFactory, ILogger<GenreOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputGenreData>> GetGenres(QueryFieldSet<OutputGenreData> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<OutputGenreData, OutputGenreData[]>("genres", fields, cancellationToken);
		}

		public async Task<OutputGenreData> GetGenre(int genreId, QueryFieldSet<OutputGenreData> fields, CancellationToken cancellationToken)
		{
			var query = Invariant($@"query GetGenreById($id: ID!) {{
										genre(id: $id) {{ {fields.QuerySelection} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					id = genreId,
				},
			};

			return await ExecuteRequest<OutputGenreData>("genre", request, cancellationToken);
		}

		public async Task<int> CreateGenre(InputGenreData genreData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new genre {GenreName} ...", genreData.Name);

			var query = Invariant($@"mutation ($genre: GenreInput!) {{
										createGenre(genre: $genre) {{ newGenreId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					genre = genreData,
				},
			};

			var data = await ExecuteRequest<CreateGenreOutputData>("createGenre", request, cancellationToken);

			var newGenreId = data.NewGenreId;

			if (newGenreId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created genre {genreData.Name}");
			}

			Logger.LogInformation("Created new genre {GenreName} with id of {GenreId}", genreData.Name, newGenreId);

			return newGenreId.Value;
		}
	}
}
