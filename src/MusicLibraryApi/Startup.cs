using System;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Dal.EfCore;
using MusicLibraryApi.GraphQL;
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

			var connectionString = Configuration.GetConnectionString("musicLibraryDB");
			if (String.IsNullOrWhiteSpace(connectionString))
			{
				throw new InvalidOperationException("Database connection string is not set");
			}

			services.AddDal(connectionString);

			services.AddSingleton<MusicLibraryQuery>();
			services.AddSingleton<MusicLibraryMutation>();
			services.AddSingleton<MusicLibrarySchema>();

			services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

			services.AddGraphQL(options => { options.ExposeExceptions = false; })
				.AddGraphTypes(ServiceLifetime.Singleton);

			services.AddTransient<IGraphQLExecuter<MusicLibrarySchema>, CustomGraphQLExecuter>();

			// Fix for the error "Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead."
			// See https://stackoverflow.com/questions/55052319/net-core-3-preview-synchronous-operations-are-disallowed
			services.Configure<IISServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});
		}

		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseGraphQL<MusicLibrarySchema>();
			app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

			var sp = app.ApplicationServices;
			ErrorHandlingMiddleware.Logger = sp?.GetService<ILogger>();
		}
	}
}
