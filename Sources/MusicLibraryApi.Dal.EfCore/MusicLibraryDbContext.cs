using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<Genre> Genres { get; set; }

		public DbSet<Disc> Discs { get; set; }

		public DbSet<Song> Songs { get; set; }

		public MusicLibraryDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Disc>().ToTable("Discs");
			modelBuilder.Entity<Disc>().Property(e => e.Title).IsRequired();
			modelBuilder.Entity<Disc>()
				.HasOne(d => d.Folder)
				.WithMany(f => f.Discs)
				.IsRequired();

			modelBuilder.Entity<Song>().ToTable("Songs");
			modelBuilder.Entity<Song>().Property(e => e.Title).IsRequired();
			modelBuilder.Entity<Song>()
				.HasOne(s => s.Disc)
				.WithMany(d => d.Songs)
				.IsRequired();

			modelBuilder.Entity<Artist>().ToTable("Artists");
			modelBuilder.Entity<Artist>().Property(e => e.Name).IsRequired();

			modelBuilder.Entity<Folder>().ToTable("Folders");
			modelBuilder.Entity<Folder>().Property(e => e.Name).IsRequired();

			modelBuilder.Entity<Genre>().ToTable("Genres");
			modelBuilder.Entity<Genre>().HasIndex(e => e.Name).IsUnique();
			modelBuilder.Entity<Genre>().Property(e => e.Name).IsRequired();

			modelBuilder.Entity<Playback>().ToTable("Playbacks");
			modelBuilder.Entity<Playback>()
				.HasOne(p => p.Song)
				.WithMany(s => s.Playbacks)
				.IsRequired();
		}
	}
}
