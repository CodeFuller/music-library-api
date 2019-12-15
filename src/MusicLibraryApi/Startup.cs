using System;
using CF.Library.Logging;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Dal.EfCore;
using MusicLibraryApi.GraphQL;
using MusicLibraryApi.Interfaces;
using MusicLibraryApi.Internal;
using MusicLibraryApi.Logic.Extensions;

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

			// ServiceAccessor should be registered with Scoped (or Transient) lifetime.
			// If it is registered with Singleton lifetime, then all instances created via inner ServiceProvider will be disposed only on application exit.
			services.AddScoped<IServiceAccessor, ServiceAccessor>();

			var connectionString = Configuration.GetConnectionString("musicLibraryDB");
			if (String.IsNullOrWhiteSpace(connectionString))
			{
				throw new InvalidOperationException("Database connection string is not set");
			}

			services.AddLogic();
			services.AddDal(connectionString);

			services.AddScoped<MusicLibraryQuery>();
			services.AddScoped<MusicLibraryMutation>();
			services.AddScoped<MusicLibrarySchema>();

			services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

			services.AddGraphQL(options => { options.ExposeExceptions = false; })
				.AddDataLoader()
				.AddGraphTypes(ServiceLifetime.Scoped);

			services.AddTransient<IGraphQLExecuter<MusicLibrarySchema>, CustomGraphQLExecuter>();

			// Fix for the error "Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead."
			// See https://stackoverflow.com/questions/55052319/net-core-3-preview-synchronous-operations-are-disallowed
			services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
			services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			loggerFactory.LoadLoggingConfiguration(Configuration);

			app.UseGraphQL<MusicLibrarySchema>();
			app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

			var sp = app.ApplicationServices;
			ErrorHandlingMiddleware.Logger = sp?.GetService<ILogger>();
		}
	}
}
