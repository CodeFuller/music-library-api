using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class ArtistType : ObjectGraphType<Artist>
	{
		public ArtistType(IContextServiceAccessor serviceAccessor)
		{
			Field(x => x.Id);
			Field(x => x.Name);
			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<SongType>>>>(
				"songs",
				resolve: async context => await serviceAccessor.SongsService.GetArtistSongs(context.Source.Id, context.CancellationToken));
		}
	}
}
