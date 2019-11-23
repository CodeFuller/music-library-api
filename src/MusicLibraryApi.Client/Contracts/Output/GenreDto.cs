namespace MusicLibraryApi.Client.Contracts.Output
{
	public class GenreDto
	{
		public int? Id { get; }

		public string? Name { get; }

		public GenreDto(int? id, string? name)
		{
			Id = id;
			Name = name;
		}
	}
}
