using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Client;
using MusicLibraryApi.Dal.EfCore;
using MusicLibraryApi.Dal.EfCore.Repositories;
using MusicLibraryApi.IntegrationTests.Utility;

namespace MusicLibraryApi.IntegrationTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Startup>, IHttpClientFactory
	{
		private readonly Action<IServiceCollection> configureServicesAction;

		public string? FileSystemStorageRoot { get; private set; }

		public CustomWebApplicationFactory(Action<IServiceCollection> configureServices)
		{
			this.configureServicesAction = configureServices ?? throw new ArgumentNullException(nameof(configureServices));

			// Fix for the error "Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead."
			// See https://stackoverflow.com/questions/55052319/net-core-3-preview-synchronous-operations-are-disallowed
			Server.AllowSynchronousIO = true;
		}

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			// This is required for tests on error handling.
			// ErrorInfoProviderOptions.ExposeExceptionStackTrace is set to environment.IsDevelopment().
			builder.UseEnvironment(Environments.Production);

			builder.ConfigureAppConfiguration((context, configBuilder) =>
			{
				FileSystemStorageRoot = Path.Combine(Path.GetTempPath(), "MusicLibraryApi.IT", Path.GetRandomFileName());

				configBuilder.AddJsonFile(Paths.GetTestRunSettingsPath(), optional: false)
					.AddInMemoryCollection(new[] { new KeyValuePair<string, string>("fileSystemStorage:root", FileSystemStorageRoot) });
			});

			builder.ConfigureServices(services =>
			{
				services.AddMusicLibraryApiClient(settings => { });
				services.AddSingleton<IHttpClientFactory>(this);

				configureServicesAction(services);
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

			DeleteDatabaseData(context);

			var identityInsert = new PostgreSqlIdentityInsert();

			SeedFoldersData(context, identityInsert);
			SeedGenresData(context, identityInsert);
			SeedArtistsData(context, identityInsert);
			SeedDiscsData(context, identityInsert);
			SeedSongsData(context, identityInsert);
			SeedPlaybacksData(context, identityInsert);

			SeedStorageData();
		}

		public void CleanData()
		{
			DeleteStorageData();

			using var serviceScope = Services.CreateScope();
			var servicesProvider = serviceScope.ServiceProvider;

			using var context = servicesProvider.GetRequiredService<MusicLibraryDbContext>();

			DeleteDatabaseData(context);
		}

		private static void DeleteDatabaseData(MusicLibraryDbContext context)
		{
			// Deleting any existing data.
			// This should be done before resetting current identity value.
			context.Playbacks.RemoveRange(context.Playbacks);
			context.Songs.RemoveRange(context.Songs);
			context.Discs.RemoveRange(context.Discs);
			context.Folders.RemoveRange(context.Folders.Where(f => f.Id != FoldersRepository.RootFolderId));
			context.Artists.RemoveRange(context.Artists);
			context.Genres.RemoveRange(context.Genres);
			context.SaveChanges();
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
		 *			Song "01 - Highway To Hell.mp3" (Song id: 2), ID3v2.3 tag presents (tag data differs from song data).
		 *			Song "02 - Hell's Bells.mp3" (Song id: 1)
		 *			Song "03 - Are You Ready?.mp3" (Song id: 3)
		 *		Disc "Some deleted disc" (Disc id: 6, deleted)
		 */
		private static void SeedFoldersData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var folder2 = new Folder { Id = 2, Name = "Russian", ParentFolderId = 1, };
			var folder3 = new Folder { Id = 3, Name = "Foreign", ParentFolderId = 1, };
			var folder4 = new Folder { Id = 4, Name = "Rammstein", ParentFolderId = 3, };
			var folder5 = new Folder { Id = 5, Name = "Guano Apes", ParentFolderId = 1, };
			var folder6 = new Folder { Id = 6, Name = "Some subfolder", ParentFolderId = 5, };
			var folder7 = new Folder { Id = 7, Name = "Empty folder", ParentFolderId = 5, };

			identityInsert.InitializeIdentityInsert(context, "Folders");

			context.Folders.AddRange(folder2, folder3, folder4, folder5, folder6, folder7);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Folders", 8);
		}

		private static void SeedGenresData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var genre1 = new Genre { Id = 1, Name = "Russian Rock", };
			var genre2 = new Genre { Id = 2, Name = "Nu Metal", };
			var genre3 = new Genre { Id = 3, Name = "Alternative Rock", };

			identityInsert.InitializeIdentityInsert(context, "Genres");

			context.Genres.AddRange(genre1, genre2, genre3);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Genres", 4);
		}

		private static void SeedArtistsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var artist1 = new Artist { Id = 1, Name = "Nautilus Pompilius", };
			var artist2 = new Artist { Id = 2, Name = "AC/DC", };
			var artist3 = new Artist { Id = 3, Name = "Кино", };

			identityInsert.InitializeIdentityInsert(context, "Artists");

			context.Artists.AddRange(artist1, artist2, artist3);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Artists", 4);
		}

		private static void SeedDiscsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var disc1 = new Disc
			{
				Id = 1,
				Year = 2001,
				Title = "Platinum Hits (CD 2)",
				TreeTitle = "2001 - Platinum Hits (CD 2)",
				AlbumTitle = "Platinum Hits",
				FolderId = 1,
				AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
				AlbumOrder = 2,
			};

			var disc2 = new Disc
			{
				Id = 2,
				Year = 2001,
				Title = "Platinum Hits (CD 1)",
				TreeTitle = "2001 - Platinum Hits (CD 1)",
				AlbumTitle = "Platinum Hits",
				FolderId = 1,
				AlbumId = "{BA39AF8F-19D4-47C7-B3CA-E294CDB18D5A}",
				AlbumOrder = 1,
			};

			var disc3 = new Disc
			{
				Id = 3,
				Year = 2000,
				Title = "Don't Give Me Names",
				TreeTitle = "2000 - Don't Give Me Names",
				AlbumTitle = "Don't Give Me Names",
				FolderId = 5,
			};

			var disc4 = new Disc
			{
				Id = 4,
				Year = null,
				Title = "Rarities",
				TreeTitle = "Rarities",
				AlbumTitle = String.Empty,
				FolderId = 5,
			};

			var disc5 = new Disc
			{
				Id = 5,
				Year = 1997,
				Title = "Proud Like A God",
				TreeTitle = "1997 - Proud Like A God",
				AlbumTitle = "Proud Like A God",
				FolderId = 5,
			};

			var disc6 = new Disc
			{
				Id = 6,
				Year = null,
				Title = "Some deleted disc",
				TreeTitle = "2007 - Some deleted disc",
				AlbumTitle = "Some deleted disc",
				FolderId = 1,
				DeleteDate = new DateTimeOffset(2019, 12, 03, 07, 56, 06, TimeSpan.FromHours(2)),
			};

			var disc7 = new Disc
			{
				Id = 7,
				Year = 2006,
				Title = "Lost (T)apes",
				TreeTitle = "2006 - Lost (T)apes",
				AlbumTitle = "Lost (T)apes",
				FolderId = 5,
				DeleteDate = new DateTimeOffset(2019, 12, 03, 07, 57, 01, TimeSpan.FromHours(2)),
				DeleteComment = "Deleted for a test",
			};

			identityInsert.InitializeIdentityInsert(context, "Discs");

			context.Discs.AddRange(disc1, disc2, disc3, disc4, disc5, disc6, disc7);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Discs", 8);
		}

		private static void SeedSongsData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var song1 = new Song
			{
				Id = 1,
				Title = "Hell's Bells",
				TreeTitle = "02 - Hell's Bells.mp3",
				TrackNumber = 2,
				Duration = new TimeSpan(0, 5, 12),
				DiscId = 1,
				ArtistId = null,
				GenreId = 2,
				Rating = Rating.R4,
				BitRate = 320000,
				Size = 1243,
				Checksum = 0x82c04f79,
				LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)),
				PlaybacksCount = 2,
			};

			var song2 = new Song
			{
				Id = 2,
				Title = "Highway To Hell",
				TreeTitle = "01 - Highway To Hell.mp3",
				TrackNumber = 1,
				Duration = new TimeSpan(0, 3, 28),
				DiscId = 1,
				ArtistId = 2,
				GenreId = 1,
				Rating = Rating.R6,
				BitRate = 320000,
				Size = 946,
				Checksum = 0x2d39e8f7,
				LastPlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)),
				PlaybacksCount = 1,
			};

			var song3 = new Song
			{
				Id = 3,
				Title = "Are You Ready?",
				TreeTitle = "03 - Are You Ready?.mp3",
				TrackNumber = null,
				Duration = new TimeSpan(0, 4, 09),
				DiscId = 2,
				ArtistId = 1,
				GenreId = null,
				Size = 673,
				Checksum = 0x8ee21503,
			};

			var song4 = new Song
			{
				Id = 4,
				Title = "Some Deleted Song",
				TreeTitle = "04 - Some Deleted Song.mp3",
				TrackNumber = 4,
				Duration = new TimeSpan(0, 7, 57),
				DiscId = 1,
				ArtistId = 3,
				GenreId = 2,
				Rating = Rating.R6,
				BitRate = 320000,
				LastPlaybackTime = new DateTimeOffset(2019, 12, 14, 17, 27, 04, TimeSpan.FromHours(2)),
				PlaybacksCount = 1,
				DeleteDate = new DateTimeOffset(2019, 12, 14, 17, 27, 05, TimeSpan.FromHours(2)),
				DeleteComment = "Deleted for a test",
			};

			identityInsert.InitializeIdentityInsert(context, "Songs");

			context.Songs.AddRange(song1, song2, song3, song4);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Songs", 5);
		}

		private static void SeedPlaybacksData(MusicLibraryDbContext context, IIdentityInsert identityInsert)
		{
			var playback1 = new Playback { Id = 1, SongId = 1, PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 25, 17, TimeSpan.FromHours(2)), };
			var playback2 = new Playback { Id = 2, SongId = 2, PlaybackTime = new DateTimeOffset(2018, 11, 25, 08, 20, 00, TimeSpan.FromHours(2)), };
			var playback3 = new Playback { Id = 3, SongId = 1, PlaybackTime = new DateTimeOffset(2015, 10, 23, 15, 18, 43, TimeSpan.FromHours(2)), };
			var playback4 = new Playback { Id = 4, SongId = 4, PlaybackTime = new DateTimeOffset(2019, 12, 14, 17, 27, 04, TimeSpan.FromHours(2)), };

			identityInsert.InitializeIdentityInsert(context, "Playbacks");

			context.Playbacks.AddRange(playback1, playback2, playback3, playback4);
			context.SaveChanges();

			identityInsert.FinalizeIdentityInsert(context, "Playbacks", 5);
		}

		private void SeedStorageData()
		{
			DeleteStorageData();

			// Creating storage directories for the folders.
			CreateStorageDirectory(String.Empty);
			CreateStorageDirectory("Foreign");
			CreateStorageDirectory("Foreign/Rammstein");
			CreateStorageDirectory("Guano Apes");
			CreateStorageDirectory("Guano Apes/Empty folder");
			CreateStorageDirectory("Guano Apes/Some subfolder");
			CreateStorageDirectory("Russian");

			// Creating storage directories for the discs.
			CreateStorageDirectory("Guano Apes/1997 - Proud Like A God");
			CreateStorageDirectory("Guano Apes/2000 - Don't Give Me Names");
			CreateStorageDirectory("Guano Apes/Rarities");
			CreateStorageDirectory("2001 - Platinum Hits (CD 2)");
			CreateStorageDirectory("2001 - Platinum Hits (CD 2)");

			// Creating storage files for the songs.
			CopyStorageFile("01 - Highway To Hell.mp3", "2001 - Platinum Hits (CD 2)");
			CopyStorageFile("02 - Hell's Bells.mp3", "2001 - Platinum Hits (CD 2)");
			CopyStorageFile("03 - Are You Ready.mp3", "2001 - Platinum Hits (CD 2)");
		}

		private void DeleteStorageData()
		{
			if (FileSystemStorageRoot == null || !Directory.Exists(FileSystemStorageRoot))
			{
				return;
			}

			// We cannot call Directory.Delete(FileSystemStorageRoot, true), because the directory contains read-only files.
			var directory = new DirectoryInfo(FileSystemStorageRoot);

			foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
			{
				info.Attributes = FileAttributes.Normal;
			}

			directory.Delete(true);
		}

		private void CreateStorageDirectory(string relativePath)
		{
			if (FileSystemStorageRoot == null)
			{
				throw new InvalidOperationException($"{nameof(FileSystemStorageRoot)} is not set");
			}

			Directory.CreateDirectory(Path.Combine(FileSystemStorageRoot, relativePath));
		}

		private void CopyStorageFile(string sourceFileName, string targetDirectoryPath)
		{
			if (FileSystemStorageRoot == null)
			{
				throw new InvalidOperationException($"{nameof(FileSystemStorageRoot)} is not set");
			}

			var sourceFilePath = Paths.GetTestDataFilePath(sourceFileName);
			var targetFilePath = Path.Combine(FileSystemStorageRoot, targetDirectoryPath, sourceFileName);
			File.Copy(sourceFilePath, targetFilePath);
		}
	}
}
