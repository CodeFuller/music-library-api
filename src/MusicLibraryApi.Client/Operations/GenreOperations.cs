using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class GenreOperations : BasicQuery, IGenresQuery, IGenresMutation
	{
		public GenreOperations(ILogger<GenreOperations> logger, IOptions<ApiConnectionSettings> options)
			: base(logger, options)
		{
		}

		public async IAsyncEnumerable<OutputGenreData> GetGenres(QueryFieldSet fields, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			var genres = await ExecuteQuery<OutputGenreData[]>("genres", fields, cancellationToken);

			foreach (var genre in genres)
			{
				yield return genre;
			}
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

			if (data.NewGenreId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created genre {genreData.Name}");
			}

			Logger.LogInformation("Created new genre {GenreName} with id of {GenreId}", genreData.Name, data.NewGenreId);

			return data.NewGenreId.Value;
		}
	}
}
