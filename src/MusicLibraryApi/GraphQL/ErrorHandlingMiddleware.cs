using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using static System.FormattableString;

namespace MusicLibraryApi.GraphQL
{
	public class ErrorHandlingMiddleware
	{
		private readonly ILogger logger;

		public ErrorHandlingMiddleware(ILogger logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<object> Resolve(ResolveFieldContext context, FieldMiddlewareDelegate next)
		{
			try
			{
				return await next(context);
			}
			catch (ServiceOperationFailedException e)
			{
				throw new ExecutionError(e.Message, e);
			}
			catch (Exception e) when (!(e is ExecutionError))
			{
				logger?.LogError(e, "Caught unhandled exception when processing the field {FieldName}", context.FieldName);
				throw new ExecutionError(Invariant($"Caught unhandled exception when processing the field '{context.FieldName}'"));
			}
		}
	}
}
