using System;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryQuery : ObjectGraphType
	{
		public MusicLibraryQuery(IContextRepositoryAccessor repositoryAccessor, ILogger<MusicLibraryMutation> logger)
		{
			FieldAsync<ListGraphType<GenreType>>(
				"genres",
				resolve: async context => await repositoryAccessor.GenresRepository.GetAllGenres(context.CancellationToken));

			FieldAsync<DiscType>(
				"disc",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var discId = context.GetArgument<int>("id");

					try
					{
						return await repositoryAccessor.DiscsRepository.GetDisc(discId, context.CancellationToken);
					}
					catch (NotFoundException e)
					{
						logger.LogError(e, "The disc with id of {DiscId} does not exist", discId);
						throw new ExecutionError(Invariant($"The disc with id of '{discId}' does not exist"));
					}
				});

			FieldAsync<ListGraphType<DiscType>>(
				"discs",
				resolve: async context => await repositoryAccessor.DiscsRepository.GetAllDiscs(context.CancellationToken));

			// This 'error' field was added for IT purpose.
			// It is required for testing of error handling middleware that hides internal sensitive exceptions.
			Field<StringGraphType>(
				"error",
				resolve: context => throw new InvalidOperationException("Some internal sensitive information goes here"));
		}
	}
}
