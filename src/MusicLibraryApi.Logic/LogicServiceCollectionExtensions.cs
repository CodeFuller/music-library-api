using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.Logic
{
	public static class LogicServiceCollectionExtensions
	{
		public static IServiceCollection AddLogic(this IServiceCollection services)
		{
			services.AddTransient<IFoldersService, FoldersService>();

			return services;
		}
	}
}
