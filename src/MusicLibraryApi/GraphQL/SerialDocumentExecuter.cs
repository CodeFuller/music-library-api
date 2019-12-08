using GraphQL;
using GraphQL.Execution;
using GraphQL.Language.AST;

namespace MusicLibraryApi.GraphQL
{
	// https://github.com/graphql-dotnet/graphql-dotnet/issues/863
	// By the default, sub-queries are executed in parallel.
	// Since single instance of DbContext is used for query processing (scoped life-time), the following error could happen:
	// System.InvalidOperationException: A second operation started on this context before a previous operation completed
	// That's why we force serial execution of sub-queries.
	public class SerialDocumentExecuter : DocumentExecuter
	{
		protected override IExecutionStrategy SelectExecutionStrategy(ExecutionContext context)
		{
			return context.Operation.OperationType == OperationType.Query ? new SerialExecutionStrategy() : base.SelectExecutionStrategy(context);
		}
	}
}
