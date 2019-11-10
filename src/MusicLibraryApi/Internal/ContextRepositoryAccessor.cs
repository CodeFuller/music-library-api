using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.Internal
{
	// https://github.com/graphql-dotnet/graphql-dotnet/issues/648#issuecomment-431489339
	public class ContextRepositoryAccessor : IContextRepositoryAccessor
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		public IGenresRepository GenresRepository => GetRepository<IGenresRepository>();

		public IDiscsRepository DiscsRepository => GetRepository<IDiscsRepository>();

		public ContextRepositoryAccessor(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		private TRepository GetRepository<TRepository>()
		{
			return httpContextAccessor.HttpContext.RequestServices.GetRequiredService<TRepository>();
		}
	}
}
