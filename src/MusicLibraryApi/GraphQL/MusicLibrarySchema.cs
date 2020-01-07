using GraphQL;
using GraphQL.Types;
using GraphQL.Upload.AspNetCore;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibrarySchema : Schema
	{
		public MusicLibrarySchema(IDependencyResolver resolver)
			: base(resolver)
		{
			Query = resolver.Resolve<MusicLibraryQuery>();
			Mutation = resolver.Resolve<MusicLibraryMutation>();

			RegisterValueConverter(new FormFileConverter());
		}
	}
}
