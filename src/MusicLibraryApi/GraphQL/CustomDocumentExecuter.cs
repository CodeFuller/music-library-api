using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.Extensions.Logging;

namespace MusicLibraryApi.GraphQL
{
	public class CustomDocumentExecuter : IDocumentExecuter
	{
		private readonly DocumentExecuter documentExecuter;

		private readonly ErrorHandlingMiddleware middleware;

		public CustomDocumentExecuter(DocumentExecuter documentExecuter, ILogger<CustomDocumentExecuter> logger)
		{
			_ = logger ?? throw new ArgumentNullException(nameof(logger));

			this.documentExecuter = documentExecuter ?? throw new ArgumentNullException(nameof(documentExecuter));
			this.middleware = new ErrorHandlingMiddleware(logger);
		}

		[Obsolete("This method will be removed in a future version.  Use ExecutionOptions parameter.")]
		public Task<ExecutionResult> ExecuteAsync(ISchema schema, object root, string query, string operationName, Inputs? inputs = null,
			object? userContext = null, CancellationToken cancellationToken = default, IEnumerable<IValidationRule>? rules = null)
		{
			throw new NotImplementedException();
		}

		public async Task<ExecutionResult> ExecuteAsync(ExecutionOptions options)
		{
			_ = options ?? throw new ArgumentNullException(nameof(options));

			options.FieldMiddleware.Use(next => context => middleware.Resolve(context, next));
			return await documentExecuter.ExecuteAsync(options);
		}

		public Task<ExecutionResult> ExecuteAsync(Action<ExecutionOptions> configure)
		{
			_ = configure ?? throw new ArgumentNullException(nameof(configure));

			var options = new ExecutionOptions();
			configure(options);

			return ExecuteAsync(options);
		}
	}
}
