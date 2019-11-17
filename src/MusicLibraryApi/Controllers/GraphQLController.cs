using System;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.GraphQL;
using MusicLibraryApi.Internal;

namespace MusicLibraryApi.Controllers
{
	[Route("[controller]")]
	public class GraphQLController : ControllerBase
	{
		private readonly ISchema schema;

		private readonly IDocumentExecuter documentExecuter;

		private readonly ILogger<GraphQLController> logger;

		public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogger<GraphQLController> logger)
		{
			this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
			this.documentExecuter = documentExecuter ?? throw new ArgumentNullException(nameof(schema));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] GraphQLQuery query, CancellationToken cancellationToken)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			var inputs = query.Variables.ToInputs();

			var executionOptions = new ExecutionOptions
			{
				Schema = schema,
				Query = query.Query,
				Inputs = inputs,
				CancellationToken = cancellationToken,
			};

			ErrorHandlingMiddleware.Logger = logger;
			executionOptions.FieldMiddleware.Use<ErrorHandlingMiddleware>();

			var result = await documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);
			return Ok(result);
		}
	}
}
