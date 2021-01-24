using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Converters;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using MusicLibraryApi.Client.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MusicLibraryApi.Client.Operations
{
	internal abstract class BasicQuery
	{
		public static string HttpClientName => "graphql";

		private readonly IHttpClientFactory httpClientFactory;

		private readonly IContractResolver contractResolver = new CustomContractResolver();
		private readonly JsonSerializer jsonSerializer;
		private readonly JsonMediaTypeFormatter formatter;

		protected ILogger<BasicQuery> Logger { get; }

		protected BasicQuery(IHttpClientFactory httpClientFactory, ILogger<BasicQuery> logger)
		{
			this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.jsonSerializer = new JsonSerializer
			{
				ContractResolver = contractResolver,
			};

			this.formatter = new JsonMediaTypeFormatter
			{
				SerializerSettings = new JsonSerializerSettings
				{
					ContractResolver = contractResolver,
				},
			};
		}

		protected async Task<TData> ExecuteQuery<TQuery, TData>(string queryName, QueryFieldSet<TQuery> fields, CancellationToken cancellationToken)
			where TData : class
		{
			var query = $@"{{ {queryName} {{ {fields.QuerySelection} }} }}";
			var request = new GraphQLRequest { Query = query };

			return await ExecuteRequest<TData>(queryName, request, cancellationToken);
		}

		protected async Task<TData> ExecuteRequest<TData>(string queryName, GraphQLRequest request, CancellationToken cancellationToken)
			where TData : class
		{
			Logger.LogDebug("Executing query {QueryName} ...", queryName);

			using var jsonContent = new StringContent(SerializePayload(request), Encoding.UTF8, "application/json");

			return await ProcessRequest<TData>(queryName, jsonContent, cancellationToken);
		}

		protected async Task<TData> ExecuteRequestWithUpload<TData>(string queryName, GraphQLRequest request, IDictionary<string, FileUpload> uploadedFiles, CancellationToken cancellationToken)
			where TData : class
		{
			Logger.LogDebug("Executing query with upload {QueryName} ...", queryName);

			using var content = new MultipartFormDataContent();

#pragma warning disable CA2000 // Dispose objects before losing scope - All instances of HttpContent added to content are disposed when `content` is disposed.
			var filesMap = uploadedFiles.Keys.ToDictionary(k => k, key => new[] { $"variables.{key}", });
			content.Add(new StringContent(JsonConvert.SerializeObject(filesMap)), "map");

			content.Add(new StringContent(SerializePayload(request)), "operations");

			foreach (var file in uploadedFiles)
			{
				var fileUpload = file.Value;
				content.Add(new StreamContent(fileUpload.Stream), $"\"{file.Key}\"", $"\"{fileUpload.FileName}\"");
			}
#pragma warning restore CA2000 // Dispose objects before losing scope

			return await ProcessRequest<TData>(queryName, content, cancellationToken);
		}

		private async Task<TData> ProcessRequest<TData>(string queryName, HttpContent content, CancellationToken cancellationToken)
		{
			using var httpClient = httpClientFactory.CreateClient(HttpClientName);
			var httpResponse = await httpClient.PostAsync(new Uri("graphql", UriKind.Relative), content, cancellationToken);

			if (!httpResponse.IsSuccessStatusCode)
			{
				Logger.LogError("GraphQL request failed with status code {RequestStatusCode}", httpResponse.StatusCode);
				throw new GraphQLRequestFailedException($"GraphQL request failed with status code {httpResponse.StatusCode}");
			}

			var responseData = await httpResponse.Content.ReadAsAsync<GraphQLResponse>(cancellationToken);
			Logger.LogDebug("The query {QueryName} completed successfully", queryName);

			return HandleGraphQLResponse<TData>(queryName, responseData);
		}

		private TData HandleGraphQLResponse<TData>(string queryName, GraphQLResponse response)
		{
			foreach (var error in response.Errors ?? Enumerable.Empty<GraphQLError>())
			{
				Logger.LogWarning("The query {QueryName} results contain error: {QueryErrorMessage}", queryName, error.Message);
			}

			var errorMessage = response.Errors?.Select(e => e.Message).FirstOrDefault(m => !String.IsNullOrEmpty(m));

			if (response.Data == null)
			{
				Logger.LogError("The query {QueryName} returned null data", queryName);
				throw new GraphQLRequestFailedException(errorMessage ?? $"Query {queryName} returned null data");
			}

			var dataToken = response.Data[queryName];
			if (dataToken == null)
			{
				Logger.LogError("The field {QueryName} is missing in query result", queryName);
				throw new GraphQLRequestFailedException(errorMessage ?? $"The field {queryName} is missing in query result");
			}

			if (dataToken.Type == JTokenType.Null)
			{
				Logger.LogError("The field {QueryName} is null in query result", queryName);
				throw new GraphQLRequestFailedException(errorMessage ?? $"The field {queryName} is null in query result");
			}

			return dataToken.ToObject<TData>(jsonSerializer);
		}

		private string SerializePayload<T>(T payload)
		{
			var serializerSettings = new JsonSerializerSettings
			{
				ContractResolver = contractResolver,
			};

			return JsonConvert.SerializeObject(payload, serializerSettings);
		}
	}
}
