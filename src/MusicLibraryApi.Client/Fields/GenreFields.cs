using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Fields
{
	public static class GenreFields
	{
		public static QueryField<OutputGenreData> Id { get; } = new QueryField<OutputGenreData>("id");

		public static QueryField<OutputGenreData> Name { get; } = new QueryField<OutputGenreData>("name");

		public static ComplexQueryField<OutputGenreData, OutputSongData> Songs(QueryFieldSet<OutputSongData> songFields)
		{
			return new ComplexQueryField<OutputGenreData, OutputSongData>("songs", songFields);
		}

		public static QueryFieldSet<OutputGenreData> All { get; } = Id + Name;
	}
}
