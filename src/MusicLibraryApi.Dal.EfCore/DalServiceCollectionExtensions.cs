using System.Reflection;
using AutoMapper;
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
			services.AddTransient<IDiscsRepository, DiscsRepository>();

			services.AddDbContext<MusicLibraryDbContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(MigrationsAssembly.Name)));

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			return services;
		}
	}
}
