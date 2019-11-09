namespace MusicLibraryApi.Dal.EfCore.Entities
{
	public class GenreEntity
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public GenreEntity(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
