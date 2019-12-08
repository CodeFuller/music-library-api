using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.IntegrationTests.Comparers.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class GenreDataComparer : BasicDataComparer<OutputGenreData>, IGenreDataComparer
	{
		private readonly IComparer<IReadOnlyCollection<OutputSongData>?> songCollectionsComparer;

		protected override IEnumerable<Func<OutputGenreData, OutputGenreData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Name);
				yield return FieldComparer(x => x.Songs, songCollectionsComparer);
			}
		}

		public GenreDataComparer(ISongDataComparer songsComparer)
		{
			this.songCollectionsComparer = new CollectionsComparer<OutputSongData>(songsComparer);
		}
	}
}
