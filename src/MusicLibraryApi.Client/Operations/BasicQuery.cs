using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client.Exceptions;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.GraphQL;
using Newtonsoft.Json;

namespace MusicLibraryApi.Client.Operations
{
	public abstract class BasicQuery
	{
		private const string GraphQLRelativeUri = "graphql";

		public static string HttpClientName => "graphql";

		private readonly IHttpClientFactory httpClientFactory;

		protected ILogger<BasicQuery> Logger { get; }

		protected BasicQuery(IHttpClientFactory httpClientFactory, ILogger<BasicQuery> logger)
		{
			this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected async Task<TData> ExecuteQuery<TData>(string queryName, QueryFieldSet fields, CancellationToken cancellationToken)
			where TData : class
		{
			var requestedFields = JoinRequestFields(fields);
			var query = $@"{{ {queryName} {{ {requestedFields} }} }}";
			var request = new GraphQLRequest { Query = query };

			return await ExecuteRequest<TData>(queryName, request, cancellationToken);
		}

		protected async Task<TData> ExecuteRequest<TData>(string queryName, GraphQLRequest request, CancellationToken cancellationToken)
			where TData : class
		{
			Logger.LogDebug("Executing query {QueryName} ...", queryName);

			using var httpClient = httpClientFactory.CreateClient(HttpClientName);
			using var httpContent = BuildJsonContent(request);
			var httpResponse = await httpClient.PostAsync(GraphQLRelativeUri, httpContent, cancellationToken);

			if (!httpResponse.IsSuccessStatusCode)
			{
				Logger.LogError("GraphQL request failed with status code {RequestStatusCode}", httpResponse.StatusCode);
				throw new GraphQLRequestFailedException($"GraphQL request failed with status code {httpResponse.StatusCode}");
			}

			var responseData = await httpResponse.Content.ReadAsAsync<GraphQLResponse>(cancellationToken);
			Logger.LogDebug("The query {QueryName} completed successfully", queryName);

			foreach (var error in responseData.Errors ?? Enumerable.Empty<GraphQLError>())
			{
				Logger.LogWarning("The query {QueryName} results contain error: {QueryErrorMessage}", queryName, error.Message);
			}

			var errorMessage = responseData.Errors?.Select(e => e.Message).FirstOrDefault(m => !String.IsNullOrEmpty(m));

			if (responseData.Data == null)
			{
				Logger.LogError("The query {QueryName} returned null data", queryName);
				throw new GraphQLRequestFailedException(errorMessage ?? $"Query {queryName} returned null data");
			}

			var dataToken = responseData.Data[queryName];
			if (dataToken == null)
			{
				Logger.LogError("The field {QueryName} is missing in query result", queryName);
				throw new GraphQLRequestFailedException(errorMessage ?? $"The field {queryName} is missing in query result");
			}

			if (!dataToken.HasValues)
			{
				Logger.LogError("The field {QueryName} is null in query result", queryName);
				throw new GraphQLRequestFailedException(errorMessage ?? $"The field {queryName} is null in query result");
			}

			return dataToken.ToObject<TData>();
		}

		private static StringContent BuildJsonContent(object requestData)
		{
			var jsonData = JsonConvert.SerializeObject(requestData);
			return new StringContent(jsonData, Encoding.UTF8, "application/json");
		}

		protected static string JoinRequestFields(QueryFieldSet fields)
		{
			return String.Join(" ", fields.Select(f => f.Name));
		}
	}
}
