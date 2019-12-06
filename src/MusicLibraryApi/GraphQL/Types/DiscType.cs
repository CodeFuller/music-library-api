using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types
{
	public class DiscType : ObjectGraphType<Disc>
	{
		public DiscType()
		{
			Field(x => x.Id);
			Field(x => x.Year, true);
			Field(x => x.Title);
			Field(x => x.TreeTitle);
			Field(x => x.AlbumTitle);
			Field(x => x.AlbumId, true);
			Field(x => x.AlbumOrder, true);
			Field(x => x.DeleteDate, true);
			Field(x => x.DeleteComment, true);
		}
	}
}
