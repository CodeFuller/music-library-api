using System;
using System.Threading.Tasks;
using GraphQL;
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

		public async Task<ExecutionResult> ExecuteAsync(ExecutionOptions options)
		{
			_ = options ?? throw new ArgumentNullException(nameof(options));

			options.FieldMiddleware.Use((_, next) => context => middleware.Resolve(context, next));
			return await documentExecuter.ExecuteAsync(options);
		}
	}
}
