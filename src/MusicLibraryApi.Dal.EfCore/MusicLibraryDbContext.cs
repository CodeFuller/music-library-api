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

				// https://stackoverflow.com/a/47930643/5740031
				b.HasIndex("ParentFolderId", nameof(FolderEntity.Name)).IsUnique();
				b.HasOne(f => f.ParentFolder)
					.WithMany(f => f!.Subfolders)
					.OnDelete(DeleteBehavior.Restrict);

				b.HasData(new FolderEntity(FoldersRepository.RootFolderId, "<ROOT>"));
			});

			modelBuilder.Entity<DiscEntity>(b =>
			{
				b.ToTable("Discs");
				b.HasOne(s => s.Folder)
					.WithMany(f => f!.Discs)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<SongEntity>(b =>
			{
				b.ToTable("Songs");
				b.HasOne(s => s.Disc)
					.WithMany(d => d.Songs)
					.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				b.HasOne(s => s.Artist)
					.WithMany(a => a!.Songs)
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
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
