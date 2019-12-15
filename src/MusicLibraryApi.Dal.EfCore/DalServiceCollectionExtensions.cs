using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;

namespace MusicLibraryApi.Dal.EfCore
{
	public static class DalServiceCollectionExtensions
	{
		public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
		{
			services.AddTransient<IDatabaseMigrator, DatabaseMigrator>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();

			services.AddDbContext<MusicLibraryDbContext>(
				(serviceProvider, options) =>
				{
					options.UseNpgsql(connectionString, b => b.MigrationsAssembly(MigrationsAssembly.Name));

					var interceptors = (serviceProvider.GetService<IEnumerable<IInterceptor>>() ?? Enumerable.Empty<IInterceptor>()).ToList();
					if (interceptors.Any())
					{
						options.AddInterceptors(interceptors);
					}
				},
				contextLifetime: ServiceLifetime.Transient,
				optionsLifetime: ServiceLifetime.Transient);

			return services;
		}
	}
}
