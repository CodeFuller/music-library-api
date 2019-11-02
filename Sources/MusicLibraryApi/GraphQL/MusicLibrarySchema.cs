using GraphQL;
using GraphQL.Types;

namespace MusicLibraryApi.GraphQL
{
	public class MusicLibrarySchema : Schema
	{
		public MusicLibrarySchema(IDependencyResolver resolver)
			: base(resolver)
		{
			Query = resolver.Resolve<MusicLibraryQuery>();
		}
	}
}
