using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class DiscType : ObjectGraphType<Disc>
	{
		public DiscType(IContextServiceAccessor serviceAccessor)
		{
			Field(x => x.Id);
			Field(x => x.Year, true);
			Field(x => x.Title);
			Field(x => x.TreeTitle);
			Field(x => x.AlbumTitle);
			Field(x => x.AlbumId, true);
			Field(x => x.AlbumOrder, true);
			Field<NonNullGraphType<FolderType>>("folder", resolve: context => context.Source.Folder);
			Field(x => x.DeleteDate, true);
			Field(x => x.DeleteComment, true);
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<SongType>>>>(
				"songs",
				resolve: async context => await serviceAccessor.SongsService.GetDiscSongs(context.Source.Id, context.CancellationToken));
		}
	}
}
