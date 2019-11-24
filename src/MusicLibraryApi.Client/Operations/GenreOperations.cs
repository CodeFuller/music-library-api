using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class GenreOperations : BasicQuery, IGenresQuery, IGenresMutation
	{
		public GenreOperations(IHttpClientFactory httpClientFactory, ILogger<GenreOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputGenreData>> GetGenres(QueryFieldSet<GenreQuery> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<GenreQuery, OutputGenreData[]>("genres", fields, cancellationToken);
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
