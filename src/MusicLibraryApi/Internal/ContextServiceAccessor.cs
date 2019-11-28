using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.Internal
{
	// https://github.com/graphql-dotnet/graphql-dotnet/issues/648#issuecomment-431489339
	public class ContextServiceAccessor : IContextServiceAccessor
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		public IGenresService GenresService => GetService<IGenresService>();

		public IArtistsService ArtistsService => GetService<IArtistsService>();

		public IFoldersService FoldersService => GetService<IFoldersService>();

		public IDiscsService DiscsService => GetService<IDiscsService>();

		public ContextServiceAccessor(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		private TService GetService<TService>()
		{
			return httpContextAccessor.HttpContext.RequestServices.GetRequiredService<TService>();
		}
	}
}
