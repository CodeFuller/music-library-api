using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Fields
{
	public static class ArtistFields
	{
		public static QueryField<OutputArtistData> Id { get; } = new QueryField<OutputArtistData>("id");

		public static QueryField<OutputArtistData> Name { get; } = new QueryField<OutputArtistData>("name");

		public static ComplexQueryField<OutputArtistData, OutputSongData> Songs(QueryFieldSet<OutputSongData> songFields)
		{
			return new ComplexQueryField<OutputArtistData, OutputSongData>("songs", songFields);
		}

		public static QueryFieldSet<OutputArtistData> All { get; } = Id + Name;
	}
}
