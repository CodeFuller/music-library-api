using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
	internal class PlaybackOperations : BasicQuery, IPlaybacksQuery, IPlaybacksMutation
	{
		public PlaybackOperations(IHttpClientFactory httpClientFactory, ILogger<PlaybackOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputPlaybackData>> GetPlaybacks(QueryFieldSet<OutputPlaybackData> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<OutputPlaybackData, OutputPlaybackData[]>("playbacks", fields, cancellationToken);
		}

		public async Task<OutputPlaybackData> GetPlayback(int playbackId, QueryFieldSet<OutputPlaybackData> fields, CancellationToken cancellationToken)
		{
			var query = Invariant($@"query GetPlaybackById($id: ID!) {{
										playback(id: $id) {{ {fields.QuerySelection} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					id = playbackId,
				},
			};

			return await ExecuteRequest<OutputPlaybackData>("playback", request, cancellationToken);
		}

		public async Task<int> AddSongPlayback(InputPlaybackData playbackData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Adding playback {PlaybackTime} for a song {SongId} ...", playbackData.PlaybackTime, playbackData.SongId);

			var query = Invariant($@"mutation ($playback: PlaybackInput!) {{
										addSongPlayback(playback: $playback) {{ newPlaybackId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					playback = playbackData,
				},
			};

			var data = await ExecuteRequest<AddPlaybackOutputData>("addSongPlayback", request, cancellationToken);

			var newPlaybackId = data.NewPlaybackId;

			if (newPlaybackId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created playback for song {playbackData.SongId}");
			}

			Logger.LogInformation("Added new playback {PlaybackTime} for a song {SongId} with id of {PlaybackId}", playbackData.PlaybackTime, playbackData.SongId, newPlaybackId);

			return newPlaybackId.Value;
		}
	}
}
