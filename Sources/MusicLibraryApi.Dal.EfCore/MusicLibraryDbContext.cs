using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Dal.EfCore.Entities;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<GenreEntity> Genres { get; set; }

		public DbSet<FolderEntity> Folders { get; set; }

		public DbSet<DiscEntity> Discs { get; set; }

		public DbSet<SongEntity> Songs { get; set; }

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
				b.Property(e => e.Title).IsRequired();
				b.HasOne(d => d.Folder)
					.WithMany(f => f.Discs)
					.IsRequired();
			});

			modelBuilder.Entity<SongEntity>(b =>
			{
				b.ToTable("Songs");
				b.Property(e => e.Title).IsRequired();
				b.HasOne(s => s.Disc)
					.WithMany(d => d.Songs)
					.IsRequired();
			});

			modelBuilder.Entity<ArtistEntity>(b =>
			{
				b.ToTable("Artists");
				b.Property(e => e.Name).IsRequired();
			});

			modelBuilder.Entity<FolderEntity>(b =>
			{
				b.ToTable("Folders");
				b.Property(e => e.Name).IsRequired();
			});

			modelBuilder.Entity<GenreEntity>(b =>
			{
				b.ToTable("Genres");

				b.HasIndex(e => e.Name).IsUnique();
				b.Property(e => e.Name).IsRequired();
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
