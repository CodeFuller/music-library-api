using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class DataLoaderTests : GraphQLTests
	{
		private readonly CountingCommandInterceptor countingCommandInterceptor = new CountingCommandInterceptor();

		private class CountingCommandInterceptor : DbCommandInterceptor
		{
			private readonly List<string> executedCommands = new List<string>();

			public int RequestsCount => executedCommands.Count;

			public void Clear()
			{
				executedCommands.Clear();
			}

			public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
			{
				executedCommands.Add(command.CommandText);
				return base.ReaderExecuted(command, eventData, result);
			}

			public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData,
				DbDataReader result, CancellationToken cancellationToken = default)
			{
				executedCommands.Add(command.CommandText);
				return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
			}
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);

			services.AddSingleton<IInterceptor>(countingCommandInterceptor);
		}

		[TestMethod]
		public async Task GraphQl_ForNestedQueries_UsesDataLoaderWithBatching()
		{
			// Arrange

			var requestedFields = FolderFields.Name + FolderFields.Discs(DiscFields.Songs(SongFields.Id + SongFields.Artist(ArtistFields.Id + ArtistFields.Name)));

			var client = CreateClient<IFoldersQuery>();

			countingCommandInterceptor.Clear();

			// Act

			var folderData = await client.GetFolder(null, requestedFields, CancellationToken.None);

			// Assert

			// Sanity check that multiple results were processed.
			Assert.AreEqual(2, folderData.Discs?.Count);
			var songs = folderData.Discs?.SelectMany(d => d.Songs).ToList();
			Assert.AreEqual(2, songs?.Select(s => s.Artist?.Id).Where(id => id != null).Distinct().Count());

			// 1st - SELECT * FROM Folders WHERE Id = 1 from FoldersService.GetFolder(null)
			// 2nd - SELECT * FROM Discs WHERE FolderId IN (1) from DiscsService.GetDiscsByFolderIds(new[] { 1 })
			// 3rd - SELECT * FROM Songs WHERE DiscId IN (1, 2) from SongsService.GetSongsByDiscIds(new[] { 1, 2 })
			// 4th - SELECT * FROM Artists WHERE Id IN (1, 2) from ArtistsService.GetArtists(new[] { 1, 2 })
			Assert.AreEqual(4, countingCommandInterceptor.RequestsCount);
		}
	}
}
