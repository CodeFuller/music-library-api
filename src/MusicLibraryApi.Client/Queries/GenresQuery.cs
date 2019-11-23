using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Client.Contracts.Output;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.Client.Queries
{
	public class GenresQuery : BasicQuery, IGenresQuery
	{
		public GenresQuery(ILogger<GenresQuery> logger, IOptions<ApiConnectionSettings> options)
			: base(logger, options)
		{
		}

		public async IAsyncEnumerable<GenreDto> GetGenres(QueryFieldSet fields, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			var genres = await ExecuteQuery<GenreDto[]>("genres", fields, cancellationToken);

			foreach (var genre in genres)
			{
				yield return genre;
			}
		}
	}
}
