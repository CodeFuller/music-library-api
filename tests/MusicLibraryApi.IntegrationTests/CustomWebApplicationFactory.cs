using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Client;
using MusicLibraryApi.Dal.EfCore;
using MusicLibraryApi.Dal.EfCore.Entities;
using MusicLibraryApi.Dal.EfCore.Repositories;
using MusicLibraryApi.IntegrationTests.Utility;

namespace MusicLibraryApi.IntegrationTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Startup>, IHttpClientFactory
	{
		private readonly IWebApplicationConfigurator appConfigurator;

		public CustomWebApplicationFactory(IWebApplicationConfigurator appConfigurator)
		{
			this.appConfigurator = appConfigurator ?? throw new ArgumentNullException(nameof(appConfigurator));

			// Fix for the error "Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead."
			// See https://stackoverflow.com/questions/55052319/net-core-3-preview-synchronous-operations-are-disallowed
			Server.AllowSynchronousIO = true;
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureAppConfiguration((context, configBuilder) =>
			{
				var currAssembly = Assembly.GetExecutingAssembly().Location;
				var currDirectory = Path.GetDirectoryName(currAssembly);
				var configFile = Path.Combine(currDirectory ?? String.Empty, "TestRunSettings.json");

				configBuilder.AddJsonFile(configFile, optional: false);
			});

			builder.ConfigureServices(services =>
			{
				services.AddMusicLibraryApiClient();
				services.AddSingleton<IHttpClientFactory>(this);

				appConfigurator.ConfigureServices(services);
			});
		}

		public HttpClient CreateClient(string name)
		{
			return CreateClient();
		}

		public void SeedData()
		{
			Services.ApplyMigrations();

			using var serviceScope = Services.CreateScope();
			var servicesProvider = serviceScope.ServiceProvider;

			using var context = servicesProvider.GetRequiredService<MusicLibraryDbContext>();

			var identityInsert = new PostgreSqlIdentityInsert();

			// Deleting any existing data.
			// This should be done before resetting current identity value.
			context.Songs.RemoveRange(context.Songs);
			context.Discs.RemoveRange(context.Discs);
			context.Folders.RemoveRange(context.Folders.Where(f => f.Id != FoldersRepository.RootFolderId));
			context.Artists.RemoveRange(context.Artists);
			context.Genres.RemoveRange(context.Genres);
			context.Playbacks.RemoveRange(context.Playbacks);
			context.SaveChanges();

			SeedFoldersData(context, identityInsert);
			SeedGenresData(context, identityInsert);
			SeedArtistsData(context, identityInsert);
			SeedDiscsData(context, identityInsert);
			SeedSongsData(context, identityInsert);
			SeedPlaybacksData(context, identityInsert);
		}

		/*
		 * The structure of test data:
		 *
		 *	Folder <ROOT> (Folder id: 1)
		 *		Folder "Foreign" (Folder id: 3)
		 *			Folder "Rammstein" (Folder id: 4)
		 *		Folder "Guano Apes" (Folder id: 5)
		 *			Folder "Empty folder" (Folder id: 7)
		 *			Folder "Some subfolder" (Folder id: 6)
		 *			Disc "1997 - Proud Like A God" (Disc id: 5)
		 *			Disc "2000 - Don't Give Me Names" (Disc id: 3)
		 *			Disc "Rarities" (Disc id: 4)
		 *			Disc "2006 - Lost (T)apes" (Disc id: 7, deleted)
		 *		Folder "Russian" (Folder id: 2)
		 *		Disc "2001 - Platinum Hits (CD 1)" (Disc id: 2)
		 *		Disc "2001 - Platinum Hits (CD 2)" (Disc id: 1)
		 *			Song "01 - Highway To Hell.mp3" (Song id: 2)
		 *			Song "02 - Hell's Bells.mp33" (Song id: 1)
		 *			Song "03 - Are You Ready?.mp3" (Song id: 3)
		 *		Disc "Some deleted disc" (Disc id: 6, deleted)
		 */
		private static void SeedFoldersData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var folder2 = new FolderEntity(2, "Russian", 1);
			var folder3 = new FolderEntity(3, "Foreign", 1);
			var folder4 = new FolderEntity(4, "Rammstein", 3);
			var folder5 = new FolderEntity(5, "Guano Apes", 1);
			var folder6 = new FolderEntity(6, "Some subfolder", 5);
			var folder7 = new FolderEntity(7, "Empty folder", 5);

			identityInsert.InitializeIdentityInsert(context, "Folders");

			context.Folders.AddRange(folder2, folder3, folder4, folder5, folder6, folder7);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Folders", 8);
		}

		private static void SeedGenresData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var genre1 = new GenreEntity(1, "Russian Rock");
			var genre2 = new GenreEntity(2, "Nu Metal");
			var genre3 = new GenreEntity(3, "Alternative Rock");

			identityInsert.InitializeIdentityInsert(context, "Genres");

			context.Genres.AddRange(genre1, genre2, genre3);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Genres", 4);
		}

		private static void SeedArtistsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var artist1 = new ArtistEntity(1, "Nautilus Pompilius");
			var artist2 = new ArtistEntity(2, "AC/DC");
			var artist3 = new ArtistEntity(3, "Кино");

			identityInsert.InitializeIdentityInsert(context, "Artists");

			context.Artists.AddRange(artist1, artist2, artist3);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Artists", 4);
		}

		private static void SeedDiscsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var disc1 = new DiscEntity(1, 2001, "Platinum Hits (CD 2)", "2001 - Platinum Hits (CD 2)", "Platinum Hits", 1, "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 2);
			var disc2 = new DiscEntity(2, 2001, "Platinum Hits (CD 1)", "2001 - Platinum Hits (CD 1)", "Platinum Hits", 1, "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}", 1);
			var disc3 = new DiscEntity(3, 2000, "Don't Give Me Names", "2000 - Don't Give Me Names", "Don't Give Me Names", 5);
			var disc4 = new DiscEntity(4, null, "Rarities", "Rarities", String.Empty, 5);
			var disc5 = new DiscEntity(5, 1997, "Proud Like A God", "1997 - Proud Like A God", "Proud Like A God", 5);
			var disc6 = new DiscEntity(6, null, "Some deleted disc", "2007 - Some deleted disc", "Some deleted disc", 1, null, null, new DateTimeOffset(2019, 12, 03, 07, 56, 06, TimeSpan.FromHours(2)));
			var disc7 = new DiscEntity(7, 2006, "Lost (T)apes", "2006 - Lost (T)apes", "Lost (T)apes", 5, null, null, new DateTimeOffset(2019, 12, 03, 07, 57, 01, TimeSpan.FromHours(2)), "Deleted for a test");

			identityInsert.InitializeIdentityInsert(context, "Discs");

			context.Discs.AddRange(disc1, disc2, disc3, disc4, disc5, disc6, disc7);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Discs", 8);
		}

		private static void SeedSongsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var song1 = new SongEntity(1, "Hell's Bells", "02 - Hell's Bells.mp3", 2, new TimeSpan(0, 5, 12), 1, null, 2,
				Rating.R4, 320000, new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), 2);

			var song2 = new SongEntity(2, "Highway To Hell", "01 - Highway To Hell.mp3", 1, new TimeSpan(0, 3, 28), 1, 2, 1,
				Rating.R6, 320000, new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), 1);

			var song3 = new SongEntity(3, "Are You Ready?", "03 - Are You Ready?.mp3", null, new TimeSpan(0, 4, 09), 1, 1, null,
				null, null, null, 0);

			identityInsert.InitializeIdentityInsert(context, "Songs");

			context.Songs.AddRange(song1, song2, song3);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Songs", 4);
		}

		private static void SeedPlaybacksData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var playback1 = new PlaybackEntity(1, 1, new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)));
			var playback2 = new PlaybackEntity(2, 2, new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)));
			var playback3 = new PlaybackEntity(3, 1, new DateTimeOffset(2015, 10, 23, 15, 18, 43, TimeSpan.FromHours(2)));

			identityInsert.InitializeIdentityInsert(context, "Playbacks");

			context.Playbacks.AddRange(playback1, playback2, playback3);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Playbacks", 4);
		}
	}
}
