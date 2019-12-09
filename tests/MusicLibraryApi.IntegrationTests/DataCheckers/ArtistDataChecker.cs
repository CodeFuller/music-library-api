using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class ArtistDataChecker : BasicDataChecker<OutputArtistData>
	{
		private readonly IDataChecker<OutputSongData> songsChecker;

		protected override IEnumerable<Action<OutputArtistData, OutputArtistData, string>> PropertiesCheckers
		{
			get
			{
				yield return FieldChecker(x => x.Id, nameof(OutputArtistData.Id));
				yield return FieldChecker(x => x.Name, nameof(OutputArtistData.Name));
				yield return FieldChecker(x => x.Songs, songsChecker, nameof(OutputArtistData.Songs));
			}
		}

		public ArtistDataChecker(IDataChecker<OutputSongData> songsChecker)
		{
			this.songsChecker = songsChecker ?? throw new ArgumentNullException(nameof(songsChecker));
		}
	}
}
