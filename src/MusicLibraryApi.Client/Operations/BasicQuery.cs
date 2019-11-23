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

namespace MusicLibraryApi.Client.Operations
{
	public abstract class BasicQuery : IDisposable
	{
		private readonly GraphQLClient graphQLClient;

		protected ILogger<BasicQuery> Logger { get; }

		protected BasicQuery(ILogger<BasicQuery> logger, IOptions<ApiConnectionSettings> options)
		{
			var settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
			if (settings.GraphQLEndpointUrl == null)
			{
				throw new InvalidOperationException($"{nameof(ApiConnectionSettings.GraphQLEndpointUrl)} is not set");
			}

			this.graphQLClient = new GraphQLClient(settings.GraphQLEndpointUrl);

			this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected async Task<TData> ExecuteQuery<TData>(string queryName, QueryFieldSet fields, CancellationToken cancellationToken)
		{
			var requestedFields = JoinRequestFields(fields);
			var query = $@"{{ {queryName} {{ {requestedFields} }} }}";
			var request = new GraphQLRequest { Query = query };

			return await ExecuteRequest<TData>(queryName, request, cancellationToken);
		}

		protected async Task<TData> ExecuteRequest<TData>(string queryName, GraphQLRequest request, CancellationToken cancellationToken)
		{
			Logger.LogDebug("Executing query {QueryName} ...", queryName);
			var response = await graphQLClient.PostAsync(request, cancellationToken);
			Logger.LogDebug("The query {QueryName} completed successfully", queryName);

			foreach (var error in response.Errors ?? Enumerable.Empty<GraphQLError>())
			{
				Logger.LogWarning("The query {QueryName} results contain error: {QueryErrorMessage}", queryName, error.Message);
			}

			if (response.Data == null)
			{
				Logger.LogError("The query {QueryName} returned null data", queryName);
				throw new GraphQLRequestFailedException($"Query {queryName} returned null data");
			}

			return response.GetDataFieldAs<TData>(queryName);
		}

		protected static string JoinRequestFields(QueryFieldSet fields)
		{
			return String.Join(" ", fields.Select(f => f.Name));
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
