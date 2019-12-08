using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.IntegrationTests.Comparers.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class ArtistDataComparer : BasicDataComparer<OutputArtistData>, IArtistDataComparer
	{
		private readonly IComparer<IReadOnlyCollection<OutputSongData>?> songCollectionsComparer;

		protected override IEnumerable<Func<OutputArtistData, OutputArtistData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Name);
				yield return FieldComparer(x => x.Songs, songCollectionsComparer);
			}
		}

		public ArtistDataComparer(ISongDataComparer songsComparer)
		{
			this.songCollectionsComparer = new CollectionsComparer<OutputSongData>(songsComparer);
		}
	}
}
