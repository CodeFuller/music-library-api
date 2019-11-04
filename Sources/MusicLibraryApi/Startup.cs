using System;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Dal.EfCore;
using MusicLibraryApi.Dal.EfCore.Repositories;
using MusicLibraryApi.GraphQL;
using MusicLibraryApi.GraphQL.Types;
using MusicLibraryApi.Interfaces;
using MusicLibraryApi.Internal;

namespace MusicLibraryApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddNewtonsoftJson();

			services.AddHttpContextAccessor();
			services.AddSingleton<IContextRepositoryAccessor, ContextRepositoryAccessor>();

			services.AddTransient<IDatabaseMigrator, DatabaseMigrator>();
			services.AddTransient<IGenresRepository, GenresRepository>();
			services.AddTransient<IDiscsRepository, DiscsRepository>();
			services.AddTransient<ISongsRepository, SongsRepository>();

			var connectionString = Configuration.GetConnectionString("musicLibraryDB");
			if (String.IsNullOrWhiteSpace(connectionString))
			{
				throw new InvalidOperationException("Database connection string is not set");
			}

			services.AddDbContext<MusicLibraryDbContext>(options => options.UseNpgsql(connectionString));

			services.AddSingleton<GenreType>();
			services.AddSingleton<GenreInputType>();
			services.AddSingleton<DiscType>();
			services.AddSingleton<SongType>();

			services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
			services.AddSingleton<MusicLibraryQuery>();
			services.AddSingleton<MusicLibraryMutation>();
			services.AddSingleton<ISchema>(sp => new MusicLibrarySchema(new FuncDependencyResolver(sp.GetService)));
		}

		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseGraphiQl();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
