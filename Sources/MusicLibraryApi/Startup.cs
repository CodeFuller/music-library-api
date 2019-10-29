using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicLibraryApi.GraphQL;

namespace MusicLibraryApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddHttpContextAccessor();

			services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
			services.AddSingleton<MusicLibraryQuery>();
			services.AddSingleton<MusicLibraryMutation>();
			services.AddSingleton<ISchema>(sp => new MusicLibrarySchema(new FuncDependencyResolver(type => sp.GetService(type))));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
