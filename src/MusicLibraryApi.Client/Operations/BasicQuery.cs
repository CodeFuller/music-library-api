using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Converters;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicLibraryApi.Client.Operations
{
	internal abstract class BasicQuery
	{
		private const string GraphQLRelativeUri = "graphql";

		public static string HttpClientName => "graphql";

		private readonly IHttpClientFactory httpClientFactory;

		private readonly JsonMediaTypeFormatter formatter;
		private readonly JsonSerializer jsonSerializer;

		protected ILogger<BasicQuery> Logger { get; }

		protected BasicQuery(IHttpClientFactory httpClientFactory, ILogger<BasicQuery> logger)
		{
			this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

			var contractResolver = new CustomContractResolver();

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

			using var httpClient = httpClientFactory.CreateClient(HttpClientName);
			var httpResponse = await httpClient.PostAsync(GraphQLRelativeUri, request, formatter, cancellationToken);

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
	}
}
