using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class ArtistEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public IReadOnlyCollection<SongEntity> Songs { get; } = new List<SongEntity>();

		public ArtistEntity(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
