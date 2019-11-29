using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Dal.EfCore.Entities;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<GenreEntity> Genres { get; set; } = null!;

		public DbSet<ArtistEntity> Artists { get; set; } = null!;

		public DbSet<FolderEntity> Folders { get; set; } = null!;

		public DbSet<DiscEntity> Discs { get; set; } = null!;

		public DbSet<SongEntity> Songs { get; set; } = null!;

		public MusicLibraryDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DiscEntity>(b =>
			{
				b.ToTable("Discs");
			});

			modelBuilder.Entity<SongEntity>(b =>
			{
				b.ToTable("Songs");
				b.HasOne(s => s.Disc)
					.WithMany(d => d.Songs)
					.IsRequired();
			});

			modelBuilder.Entity<ArtistEntity>(b =>
			{
				b.ToTable("Artists");
				b.HasIndex(e => e.Name).IsUnique();
			});

			modelBuilder.Entity<FolderEntity>(b =>
			{
				b.ToTable("Folders");

				// https://stackoverflow.com/a/47930643/5740031
				b.HasIndex("ParentFolderId", nameof(FolderEntity.Name)).IsUnique();
				b.HasOne(f => f.ParentFolder)
					.WithMany(f => f!.Subfolders);
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
					.IsRequired();
			});
		}
	}
}
