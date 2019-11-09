using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Dal.EfCore.Entities;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<GenreEntity> Genres { get; set; } = null!;

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
				b.HasOne(d => d.Folder)
					.WithMany(f => f.Discs)
					.IsRequired();
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
			});

			modelBuilder.Entity<FolderEntity>(b =>
			{
				b.ToTable("Folders");
				b.HasOne(f => f.ParentFolder)
					.WithMany(f => f!.ChildFolders);
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
