using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class GenreDataChecker : BasicDataChecker<OutputGenreData>
	{
		private readonly IDataChecker<OutputSongData> songsChecker;

		protected override IEnumerable<Action<OutputGenreData, OutputGenreData, string>> PropertiesCheckers
		{
			get
			{
				yield return FieldChecker(x => x.Id, nameof(OutputGenreData.Id));
				yield return FieldChecker(x => x.Name, nameof(OutputGenreData.Name));
				yield return FieldChecker(x => x.Songs, songsChecker, nameof(OutputGenreData.Songs));
			}
		}

		public GenreDataChecker(IDataChecker<OutputSongData> songsChecker)
		{
			this.songsChecker = songsChecker ?? throw new ArgumentNullException(nameof(songsChecker));
		}
	}
}
