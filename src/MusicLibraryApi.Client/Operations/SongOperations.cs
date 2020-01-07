using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.Client.Internal;
using static System.FormattableString;

namespace MusicLibraryApi.Client.Operations
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class SongOperations : BasicQuery, ISongsQuery, ISongsMutation
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public SongOperations(IHttpClientFactory httpClientFactory, ILogger<SongOperations> logger)
			: base(httpClientFactory, logger)
		{
		}

		public async Task<IReadOnlyCollection<OutputSongData>> GetSongs(QueryFieldSet<OutputSongData> fields, CancellationToken cancellationToken)
		{
			return await ExecuteQuery<OutputSongData, OutputSongData[]>("songs", fields, cancellationToken);
		}

		public async Task<OutputSongData> GetSong(int songId, QueryFieldSet<OutputSongData> fields, CancellationToken cancellationToken)
		{
			var query = Invariant($@"query GetSongById($id: ID!) {{
										song(id: $id) {{ {fields.QuerySelection} }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					id = songId,
				},
			};

			return await ExecuteRequest<OutputSongData>("song", request, cancellationToken);
		}

		public async Task<int> CreateSong(InputSongData songData, Stream songContent, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new song {SongTreeTitle} ...", songData.TreeTitle);

			var query = Invariant($@"mutation ($song: SongInput!, $songFile: Upload!) {{
										createSong(song: $song, songFile: $songFile) {{ newSongId }}
									}}");

			var uploadedFiles = new Dictionary<string, FileUpload>
			{
				{ "songFile", new FileUpload(songContent, songData.TreeTitle ?? String.Empty) },
			};

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					song = songData,

					// songFile (from mutation definition) -> songFile (in below map)
					songFile = "songFile",
				},
			};

			var data = await ExecuteRequestWithUpload<CreateSongOutputData>("createSong", request, uploadedFiles, cancellationToken);

			var newSongId = data.NewSongId;

			if (newSongId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created song {songData.TreeTitle}");
			}

			Logger.LogInformation("Created new song {SongTreeTitle} with id of {SongId}", songData.TreeTitle, newSongId);

			return newSongId.Value;
		}

		public async Task<int> CreateDeletedSong(InputSongData songData, CancellationToken cancellationToken)
		{
			Logger.LogInformation("Creating new (deleted) song {SongTreeTitle} ...", songData.TreeTitle);

			var query = Invariant($@"mutation ($song: SongInput!) {{
										createDeletedSong(song: $song) {{ newSongId }}
									}}");

			var request = new GraphQLRequest
			{
				Query = query,
				Variables = new
				{
					song = songData,
				},
			};

			var data = await ExecuteRequest<CreateSongOutputData>("createDeletedSong", request, cancellationToken);

			var newSongId = data.NewSongId;

			if (newSongId == null)
			{
				throw new GraphQLRequestFailedException($"Response does not contain id of created (deleted) song {songData.TreeTitle}");
			}

			Logger.LogInformation("Created new (deleted) song {SongTreeTitle} with id of {SongId}", songData.TreeTitle, newSongId);

			return newSongId.Value;
		}
	}
}
