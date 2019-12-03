using System;
using GraphQL.Types;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryQuery : ObjectGraphType
	{
		public MusicLibraryQuery(IContextServiceAccessor serviceAccessor)
		{
			FieldAsync<ListGraphType<GenreType>>(
				"genres",
				resolve: async context => await serviceAccessor.GenresService.GetAllGenres(context.CancellationToken));

			FieldAsync<ListGraphType<ArtistType>>(
				"artists",
				resolve: async context => await serviceAccessor.ArtistsService.GetAllArtists(context.CancellationToken));

			FieldAsync<FolderType>(
				"folder",
				arguments: new QueryArguments(
					new QueryArgument<IdGraphType> { Name = "folderId" },
					new QueryArgument<BooleanGraphType> { Name = "includeDeletedDiscs" }),
				resolve: async context =>
				{
					var folderId = context.GetArgument<int?>("folderId");
					var loadSubfolders = context.SubFields.ContainsKey("subfolders");
					var loadDiscs = context.SubFields.ContainsKey("discs");
					var includeDeletedDiscs = context.GetArgument<bool?>("includeDeletedDiscs") ?? false;
					return await serviceAccessor.FoldersService.GetFolder(folderId, loadSubfolders, loadDiscs, includeDeletedDiscs, context.CancellationToken);
				});

			FieldAsync<DiscType>(
				"disc",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }),
				resolve: async context =>
				{
					var discId = context.GetArgument<int>("id");
					return await serviceAccessor.DiscsService.GetDisc(discId, context.CancellationToken);
				});

			FieldAsync<ListGraphType<DiscType>>(
				"discs",
				resolve: async context => await serviceAccessor.DiscsService.GetAllDiscs(context.CancellationToken));

			// This 'error' field was added for IT purpose.
			// It is required for testing of error handling middleware that hides internal sensitive exceptions.
			Field<StringGraphType>(
				"error",
				resolve: context => throw new InvalidOperationException("Some internal sensitive information goes here"));
		}
	}
}
