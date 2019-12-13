using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Dal.EfCore.Repositories;

namespace MusicLibraryApi.Dal.EfCore
{
	public static class DalServiceCollectionExtensions
	{
		public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
		{
			services.AddTransient<IDatabaseMigrator, DatabaseMigrator>();
			services.AddTransient<IGenresRepository, GenresRepository>();
			services.AddTransient<IArtistsRepository, ArtistsRepository>();
			services.AddTransient<IFoldersRepository, FoldersRepository>();
			services.AddTransient<IDiscsRepository, DiscsRepository>();
			services.AddTransient<ISongsRepository, SongsRepository>();
			services.AddTransient<IPlaybacksRepository, PlaybacksRepository>();

			services.AddTransient<IUnitOfWork, UnitOfWork>();

			services.AddDbContext<MusicLibraryDbContext>(
				options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(MigrationsAssembly.Name)),
				contextLifetime: ServiceLifetime.Transient,
				optionsLifetime: ServiceLifetime.Transient);

			return services;
		}
	}
}
