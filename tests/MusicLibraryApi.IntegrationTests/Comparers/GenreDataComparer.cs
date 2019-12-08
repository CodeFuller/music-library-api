using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Genres;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class GenreDataComparer : BasicDataComparer<OutputGenreData>
	{
		protected override IEnumerable<Func<OutputGenreData, OutputGenreData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Name);
			}
		}
	}
}
