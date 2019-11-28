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
		public static ILogger? Logger { get; set; }

#pragma warning disable CA1822 // Mark members as static - GraphQL.NET requires that Resolve() is an instance method.
		public async Task<object> Resolve(ResolveFieldContext context, FieldMiddlewareDelegate next)
#pragma warning restore CA1822 // Mark members as static
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
				Logger?.LogError(e, "Caught unhandled exception when processing the field {FieldName}", context.FieldName);
				throw new ExecutionError(Invariant($"Caught unhandled exception when processing the field '{context.FieldName}'"));
			}
		}
	}
}
