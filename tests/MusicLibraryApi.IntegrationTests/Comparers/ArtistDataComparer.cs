using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Artists;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class ArtistDataComparer : BasicDataComparer<OutputArtistData>
	{
		protected override IEnumerable<Func<OutputArtistData, OutputArtistData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Name);
			}
		}
	}
}
