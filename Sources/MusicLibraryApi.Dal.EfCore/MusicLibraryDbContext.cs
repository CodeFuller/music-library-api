using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore
{
	public class MusicLibraryDbContext : DbContext
	{
		public DbSet<Disc> Discs { get; set; }

		public DbSet<Song> Songs { get; set; }

		public MusicLibraryDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Disc>().Property(e => e.Title).IsRequired();

			modelBuilder.Entity<Song>().Property(e => e.Title).IsRequired();
			modelBuilder.Entity<Song>()
				.HasOne(s => s.Disc)
				.WithMany(d => d.Songs)
				.IsRequired();
		}
	}
}
