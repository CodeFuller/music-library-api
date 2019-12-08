using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Fields.QueryTypes;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	public class SongOperations : BasicQuery, ISongsQuery, ISongsMutation
	{
		public SongOperations(IHttpClientFactory httpClientFactory, ILogger<BasicQuery> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputSongData>> GetSongs(QueryFieldSet<SongQuery> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<SongQuery, OutputSongData[]>("songs", fields, cancellationToken);
		}

		public async Task<int> CreateSong(InputSongData songData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new song {SongTreeTitle} ...", songData.TreeTitle);

			var query = Invariant($@"mutation ($song: SongInput!) {{
										createSong(song: $song) {{ newSongId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					song = songData,
				},
			};

			var data = await ExecuteRequest<CreateSongOutputData>("createSong", request, cancellationToken);

			var newSongId = data.NewSongId;

			if (newSongId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created song {songData.TreeTitle}");
			}

			Logger.LogInformation("Created new song {SongTreeTitle} with id of {SongId}", songData.TreeTitle, newSongId);

			return newSongId.Value;
		}
	}
}
