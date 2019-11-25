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

		private static void SeedFoldersData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var folder1 = new FolderEntity(1, "Russian");
			var folder2 = new FolderEntity(2, "Nautilus Pompilius");
			folder2.ParentFolder = folder1;

			var folder3 = new FolderEntity(3, "Сборники");
			var folder4 = new FolderEntity(4, "Best");
			folder4.ParentFolder = folder3;

			var folder5 = new FolderEntity(5, "Foreign");
			var folder6 = new FolderEntity(6, "AC-DC");
			folder6.ParentFolder = folder5;

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
			var disc1 = new DiscEntity(1, 1988, "Князь тишины");
			disc1.Folder = FindFolder(context, "Nautilus Pompilius");

			var disc2 = new DiscEntity(2, 2001, "Platinum Hits (CD 1)", "Platinum Hits", 1, new DateTimeOffset(2019, 11, 10, 15, 38, 01, TimeSpan.FromHours(2)), "Boring");
			disc2.Folder = FindFolder(context, "AC-DC");

			var disc3 = new DiscEntity(3, 2001, "Platinum Hits (CD 2)", "Platinum Hits", 2, new DateTimeOffset(2019, 11, 10, 15, 38, 02, TimeSpan.FromHours(2)), "Boring");
			disc3.Folder = FindFolder(context, "AC-DC");

			var disc4 = new DiscEntity(4, null, "Foreign");
			disc4.Folder = FindFolder(context, "Best");

			identityInsert.InitializeIdentityInsert(context, "Discs");

			context.Discs.AddRange(disc1, disc2, disc3, disc4);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Discs", 5);
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
