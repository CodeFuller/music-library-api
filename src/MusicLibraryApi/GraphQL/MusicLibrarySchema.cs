using System;
using GraphQL.Types;
using GraphQL.Upload.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibrarySchema : Schema
	{
		public MusicLibrarySchema(IServiceProvider services)
			: base(services)
		{
			Query = services.GetRequiredService<MusicLibraryQuery>();
			Mutation = services.GetRequiredService<MusicLibraryMutation>();

			RegisterValueConverter(new FormFileConverter());
		}
	}
}
