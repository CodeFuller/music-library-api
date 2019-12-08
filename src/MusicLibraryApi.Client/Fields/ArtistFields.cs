using MusicLibraryApi.Client.Fields.QueryTypes;

namespace MusicLibraryApi.Client.Fields
{
	public static class ArtistFields
	{
		public static QueryField<ArtistQuery> Id { get; } = new QueryField<ArtistQuery>("id");

		public static QueryField<ArtistQuery> Name { get; } = new QueryField<ArtistQuery>("name");

		public static ComplexQueryField<ArtistQuery, SongQuery> Songs(QueryFieldSet<SongQuery> songFields)
		{
			return new ComplexQueryField<ArtistQuery, SongQuery>("songs", songFields);
		}

		public static QueryFieldSet<ArtistQuery> All { get; } = Id + Name;
	}
}
