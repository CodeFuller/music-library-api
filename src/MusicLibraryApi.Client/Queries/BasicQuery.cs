using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicLibraryApi.Client.Exceptions;

namespace MusicLibraryApi.Client.Queries
{
	public abstract class BasicQuery : IDisposable
	{
		private readonly GraphQLClient graphQLClient;

		private readonly ILogger<BasicQuery> logger;

		protected BasicQuery(ILogger<BasicQuery> logger, IOptions<ApiConnectionSettings> options)
		{
			var settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
			if (settings.GraphQLEndpointUrl == null)
			{
				throw new InvalidOperationException($"{nameof(ApiConnectionSettings.GraphQLEndpointUrl)} is not set");
			}

			this.graphQLClient = new GraphQLClient(settings.GraphQLEndpointUrl);

			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected async Task<TData> ExecuteQuery<TData>(string queryName, QueryFieldSet fields, CancellationToken cancellationToken)
		{
			var requestedFields = String.Join(" ", fields.Select(f => f.Name));
			var query = $@"{{ {queryName} {{ {requestedFields} }} }}";
			var request = new GraphQLRequest { Query = query };

			return await ExecuteRequest<TData>(queryName, request, cancellationToken);
		}

		protected async Task<TData> ExecuteRequest<TData>(string queryName, GraphQLRequest request, CancellationToken cancellationToken)
		{
			logger.LogDebug("Executing query {QueryName} ...", queryName);
			var response = await graphQLClient.PostAsync(request, cancellationToken);
			logger.LogDebug("The query {QueryName} completed successfully", queryName);

			foreach (var error in response.Errors ?? Enumerable.Empty<GraphQLError>())
			{
				logger.LogWarning("The query {QueryName} results contain error: {QueryErrorMessage}", queryName, error.Message);
			}

			if (response.Data == null)
			{
				logger.LogError("The query {QueryName} returned null data", queryName);
				throw new GraphQLRequestFailedException($"Query {queryName} returned null data");
			}

			return response.GetDataFieldAs<TData>(queryName);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				graphQLClient?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
