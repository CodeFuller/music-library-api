﻿using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.GraphQL.Types.Input;
using MusicLibraryApi.GraphQL.Types.Output;
using MusicLibraryApi.Interfaces;
using static System.FormattableString;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibraryMutation : ObjectGraphType
	{
		public MusicLibraryMutation(IContextServiceAccessor serviceAccessor, ILogger<MusicLibraryMutation> logger)
		{
			FieldAsync<CreateGenreResultType>(
				"createGenre",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<GenreInputType>> { Name = "genre" }),
				resolve: async context =>
				{
					var genreInput = context.GetArgument<GenreInput>("genre");
					var genre = genreInput.ToModel();

					try
					{
						var newGenreId = await serviceAccessor.GenresRepository.AddGenre(genre, context.CancellationToken);
						return new CreateGenreResult(newGenreId);
					}
					catch (DuplicateKeyException e)
					{
						logger.LogError(e, "Genre {GenreName} already exists", genre.Name);
						throw new ExecutionError(Invariant($"Genre '{genre.Name}' already exists"));
					}
				});

			FieldAsync<CreateArtistResultType>(
				"createArtist",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<ArtistInputType>> { Name = "artist" }),
				resolve: async context =>
				{
					var artistInput = context.GetArgument<ArtistInput>("artist");
					var artist = artistInput.ToModel();

					try
					{
						var newArtistId = await serviceAccessor.ArtistsRepository.AddArtist(artist, context.CancellationToken);
						return new CreateArtistResult(newArtistId);
					}
					catch (DuplicateKeyException e)
					{
						logger.LogError(e, "Artist {ArtistName} already exists", artist.Name);
						throw new ExecutionError(Invariant($"Artist '{artist.Name}' already exists"));
					}
				});

			FieldAsync<CreateFolderResultType>(
				"createFolder",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<FolderInputType>> { Name = "folder" }),
				resolve: async context =>
				{
					var folderInput = context.GetArgument<FolderInput>("folder");

					try
					{
						var newFolderId = await serviceAccessor.FoldersService.CreateFolder(folderInput.ParentFolderId, folderInput.GetFolderName(), context.CancellationToken);
						return new CreateFolderResult(newFolderId);
					}
					catch (DuplicateKeyException e)
					{
						logger.LogError(e, "Folder {FolderName} already exists", folderInput.Name);
						throw new ExecutionError(Invariant($"Folder '{folderInput.Name}' already exists"));
					}
				});

			FieldAsync<CreateDiscResultType>(
				"createDisc",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<DiscInputType>> { Name = "disc" },
					new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "folderId" }),
				resolve: async context =>
				{
					var folderId = context.GetArgument<int>("folderId");
					var discInput = context.GetArgument<DiscInput>("disc");
					var disc = discInput.ToModel();

					try
					{
						var newDiscId = await serviceAccessor.DiscsRepository.AddDisc(folderId, disc, context.CancellationToken);
						return new CreateDiscResult(newDiscId);
					}
					catch (NotFoundException e)
					{
						logger.LogError(e, "The folder with id of {FolderId} does not exist", folderId);
						throw new ExecutionError(Invariant($"The folder with id of '{folderId}' does not exist"));
					}
				});
		}
	}
}
