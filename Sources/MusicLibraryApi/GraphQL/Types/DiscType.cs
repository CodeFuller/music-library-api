using System.Threading;
using GraphQL.Types;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class DiscType : ObjectGraphType<Disc>
	{
		public DiscType(IContextRepositoryAccessor repositoryAccessor)
		{
			Field(x => x.Id);
			Field(x => x.Year, true);
			Field(x => x.Title);
			Field(x => x.AlbumTitle, true);
			Field(x => x.AlbumOrder, true);
			Field<ListGraphType<SongType>>("songs",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
				resolve: context => repositoryAccessor.SongsRepository.GetDiscSongs(context.Source.Id, CancellationToken.None), description: "Disc songs");
		}
	}
}
