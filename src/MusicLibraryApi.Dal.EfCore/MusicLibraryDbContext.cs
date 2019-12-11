using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Dal.EfCore.Entities;
using MusicLibraryApi.Dal.EfCore.Repositories;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<GenreEntity> Genres { get; set; } = null!;

		public DbSet<ArtistEntity> Artists { get; set; } = null!;

		public DbSet<FolderEntity> Folders { get; set; } = null!;

		public DbSet<DiscEntity> Discs { get; set; } = null!;

		public DbSet<SongEntity> Songs { get; set; } = null!;

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

			modelBuilder.Entity<FolderEntity>(b =>
			{
				b.ToTable("Folders");

				b.HasIndex(f => new { f.ParentFolderId, f.Name, }).IsUnique();
				b.HasOne(f => f.ParentFolder)
					.WithMany(f => f!.Subfolders)
					.HasForeignKey(d => d.ParentFolderId)
					.OnDelete(DeleteBehavior.Restrict);

				b.HasData(new FolderEntity(FoldersRepository.RootFolderId, "<ROOT>", null));
			});

			modelBuilder.Entity<DiscEntity>(b =>
			{
				b.ToTable("Discs");
				b.HasOne(s => s.Folder)
					.WithMany(f => f!.Discs)
					.IsRequired()
					.HasForeignKey(d => d.FolderId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<SongEntity>(b =>
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

			modelBuilder.Entity<ArtistEntity>(b =>
			{
				b.ToTable("Artists");
				b.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<GenreEntity>(b =>
			{
				b.ToTable("Genres");
				b.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<PlaybackEntity>(b =>
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
