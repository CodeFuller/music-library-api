using System.Collections.Generic;
using System.Threading;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Instrumentation;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Validation;
using Microsoft.Extensions.Options;

namespace MusicLibraryApi.GraphQL
{
	public class CustomGraphQLExecuter : DefaultGraphQLExecuter<MusicLibrarySchema>
	{
		public CustomGraphQLExecuter(MusicLibrarySchema schema, IDocumentExecuter documentExecuter,
			IOptions<GraphQLOptions> options, IEnumerable<IDocumentExecutionListener> listeners, IEnumerable<IValidationRule> validationRules)
			: base(schema, documentExecuter, options, listeners, validationRules)
		{
		}

		protected override ExecutionOptions GetOptions(string operationName, string query, Inputs variables, object context, CancellationToken cancellationToken)
		{
			var options = base.GetOptions(operationName, query, variables, context, cancellationToken);

			options.FieldMiddleware.Use<ErrorHandlingMiddleware>();

			return options;
		}
	}
}
