using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Dal.EfCore.Repositories;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<Genre> Genres { get; set; } = null!;

		public DbSet<Artist> Artists { get; set; } = null!;

		public DbSet<Folder> Folders { get; set; } = null!;

		public DbSet<Disc> Discs { get; set; } = null!;

		public DbSet<Song> Songs { get; set; } = null!;

		public DbSet<Playback> Playbacks { get; set; } = null!;

		public static string DiscFolderForeignKeyName => "FK_Discs_Folders_FolderId";

		public static string FolderParentFolderForeignKeyName => "FK_Folders_Folders_ParentFolderId";

		public static string SongDiscForeignKeyName => "FK_Songs_Discs_DiscId";

		public static string SongArtistForeignKeyName => "FK_Songs_Artists_ArtistId";

		public static string SongGenreForeignKeyName => "FK_Songs_Genres_GenreId";

		public MusicLibraryDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Folder>(b =>
			{
				b.ToTable("Folders");

				b.HasIndex(f => new { f.ParentFolderId, f.Name, }).IsUnique();
				b.HasOne(f => f.ParentFolder)
					.WithMany(f => f!.Subfolders)
					.HasForeignKey(d => d.ParentFolderId)
					.HasConstraintName(FolderParentFolderForeignKeyName)
					.OnDelete(DeleteBehavior.Restrict);

				b.HasData(new Folder(FoldersRepository.RootFolderId, "<ROOT>", null));
			});

			modelBuilder.Entity<Disc>(b =>
			{
				b.ToTable("Discs");
				b.HasOne(s => s.Folder)
					.WithMany(f => f!.Discs)
					.IsRequired()
					.HasForeignKey(d => d.FolderId)
					.HasConstraintName(DiscFolderForeignKeyName)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Song>(b =>
			{
				b.ToTable("Songs");
				b.HasOne(s => s.Disc)
					.WithMany(d => d.Songs)
					.IsRequired()
					.HasForeignKey(s => s.DiscId)
					.HasConstraintName(SongDiscForeignKeyName)
					.OnDelete(DeleteBehavior.Restrict);

				b.HasOne(s => s.Artist)
					.WithMany(a => a!.Songs)
					.HasForeignKey(s => s.ArtistId)
					.HasConstraintName(SongArtistForeignKeyName)
					.OnDelete(DeleteBehavior.Restrict);

				b.HasOne(s => s.Genre)
					.WithMany(g => g!.Songs)
					.HasForeignKey(s => s.GenreId)
					.HasConstraintName(SongGenreForeignKeyName)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Artist>(b =>
			{
				b.ToTable("Artists");
				b.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<Genre>(b =>
			{
				b.ToTable("Genres");
				b.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<Playback>(b =>
			{
				b.ToTable("Playbacks");
				b.HasOne(p => p.Song)
					.WithMany(s => s.Playbacks)
					.IsRequired()
					.HasForeignKey(p => p.SongId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
