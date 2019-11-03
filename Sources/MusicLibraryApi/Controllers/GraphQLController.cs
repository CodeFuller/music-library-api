using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using MusicLibraryApi.Internal;

namespace MusicLibraryApi.Controllers
{
	[Route("[controller]")]
	public class GraphQLController : ControllerBase
	{
		private readonly ISchema schema;

		private readonly IDocumentExecuter documentExecuter;

		public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
		{
			this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
			this.documentExecuter = documentExecuter ?? throw new ArgumentNullException(nameof(schema));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
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
			};

			var result = await documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

			if (result.Errors?.Count > 0)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}
	}
}
