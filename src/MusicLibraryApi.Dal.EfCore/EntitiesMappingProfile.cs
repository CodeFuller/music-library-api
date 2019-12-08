using AutoMapper;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Dal.EfCore.Entities;

namespace MusicLibraryApi.Dal.EfCore
{
	public class EntitiesMappingProfile : Profile
	{
		public EntitiesMappingProfile()
		{
			CreateBidirectionalMap<Artist, ArtistEntity>();
			CreateBidirectionalMap<Disc, DiscEntity>();
			CreateBidirectionalMap<Folder, FolderEntity>();
			CreateBidirectionalMap<Genre, GenreEntity>();
			CreateBidirectionalMap<Song, SongEntity>();
			CreateBidirectionalMap<Playback, PlaybackEntity>();
		}

		private void CreateBidirectionalMap<T1, T2>()
		{
			CreateMap<T1, T2>();
			CreateMap<T2, T1>();
		}
	}
}
