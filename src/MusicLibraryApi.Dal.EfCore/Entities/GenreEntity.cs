using System.Collections.Generic;

namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class GenreEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public IReadOnlyCollection<SongEntity> Songs { get; } = new List<SongEntity>();

		public GenreEntity(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
