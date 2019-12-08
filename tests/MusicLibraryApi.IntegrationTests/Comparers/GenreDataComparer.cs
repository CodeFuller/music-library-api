using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class GenreDataComparer : BasicDataComparer<OutputGenreData>
	{
		private readonly IComparer<IReadOnlyCollection<OutputSongData>?> songCollectionsComparer = new CollectionsComparer<OutputSongData>(new SongDataComparer());

		protected override IEnumerable<Func<OutputGenreData, OutputGenreData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Name);
				yield return FieldComparer(x => x.Songs, songCollectionsComparer);
			}
		}
	}
}
