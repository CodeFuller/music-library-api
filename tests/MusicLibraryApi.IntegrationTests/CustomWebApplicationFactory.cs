using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Client;
using MusicLibraryApi.Dal.EfCore;
using MusicLibraryApi.Dal.EfCore.Entities;
using MusicLibraryApi.IntegrationTests.Utility;

namespace MusicLibraryApi.IntegrationTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Startup>, IHttpClientFactory
	{
		public CustomWebApplicationFactory()
		{
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
			context.Folders.RemoveRange(context.Folders);
			context.Artists.RemoveRange(context.Artists);
			context.Genres.RemoveRange(context.Genres);
			context.SaveChanges();

			SeedFoldersData(context, identityInsert);
			SeedGenresData(context, identityInsert);
			SeedArtistsData(context, identityInsert);
			SeedDiscsData(context, identityInsert);
			SeedSongsData(context, identityInsert);
		}

		/*
		 * The structure of test data:
		 *
		 *	Folder <ROOT> (Folder id: null)
		 *		Folder "Foreign" (Folder id: 2)
		 *			Folder "Guano Apes" (Folder id: 4)
		 *				Folder "Empty folder" (Folder id: 6)
		 *				Folder "Some subfolder" (Folder id: 5)
		 *				Disc "1997 - Proud Like A God" (Disc id: 5)
		 *				Disc "2000 - Don't Give Me Names" (Disc id: 3)
		 *				Disc "Rarities" (Disc id: 4)
		 *				Disc "2006 - Lost (T)apes" (Disc id: 7, deleted)
		 *			Folder "Rammstein" (Folder id: 3)
		 *		Folder "Russian" (Folder id: 1)
		 *		Disc "2001 - Platinum Hits (CD 1)" (Disc id: 2)
		 *		Disc "2001 - Platinum Hits (CD 2)" (Disc id: 1)
		 *		Disc "Some deleted disc" (Disc id: 6, deleted)
		 */
		private static void SeedFoldersData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var folder1 = new FolderEntity(1, "Russian");
			var folder2 = new FolderEntity(2, "Foreign");

			var folder3 = new FolderEntity(3, "Rammstein");
			folder3.ParentFolder = folder2;

			var folder4 = new FolderEntity(4, "Guano Apes");
			folder4.ParentFolder = folder2;

			var folder5 = new FolderEntity(5, "Some subfolder");
			folder5.ParentFolder = folder4;

			var folder6 = new FolderEntity(6, "Empty folder");
			folder6.ParentFolder = folder4;

			identityInsert.InitializeIdentityInsert(context, "Folders");

			context.Folders.AddRange(folder1, folder2, folder3, folder4, folder5, folder6);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Folders", 7);
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
			var disc1 = new DiscEntity(1, 2001, "Platinum Hits (CD 2)", "Platinum Hits", 2);
			disc1.Folder = null;

			var disc2 = new DiscEntity(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1);
			disc2.Folder = null;

			var disc3 = new DiscEntity(3, 2000, "Don't Give Me Names");
			disc3.Folder = FindFolder(context, "Guano Apes");

			var disc4 = new DiscEntity(4, null, "Rarities");
			disc4.Folder = FindFolder(context, "Guano Apes");

			var disc5 = new DiscEntity(5, 1997, "Proud Like A God");
			disc5.Folder = FindFolder(context, "Guano Apes");

			var disc6 = new DiscEntity(6, null, "Some deleted disc", null, null, new DateTimeOffset(2019, 12, 03, 07, 56, 06, TimeSpan.FromHours(2)));
			disc6.Folder = null;

			var disc7 = new DiscEntity(7, 2006, "Lost (T)apes", null, null, new DateTimeOffset(2019, 12, 03, 07, 57, 01, TimeSpan.FromHours(2)));
			disc7.Folder = FindFolder(context, "Guano Apes");

			identityInsert.InitializeIdentityInsert(context, "Discs");

			context.Discs.AddRange(disc1, disc2, disc3, disc4, disc5, disc6, disc7);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Discs", 8);
		}

		private static void SeedSongsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			identityInsert.InitializeIdentityInsert(context, "Songs");

			context.SaveChanges();
		}

		private static FolderEntity FindFolder(MusicLibraryDbContext context, string folderName)
		{
			return context.Folders.Single(f => f.Name == folderName);
		}
	}
}
