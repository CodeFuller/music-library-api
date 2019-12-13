using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Playbacks;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class PlaybackDataChecker : BasicDataChecker<OutputPlaybackData>
	{
		private readonly IDataChecker<OutputSongData> songsChecker;

		protected override IEnumerable<Action<OutputPlaybackData, OutputPlaybackData, string>> PropertiesCheckers
		{
			get
			{
				yield return FieldChecker(x => x.Id, nameof(OutputPlaybackData.Id));
				yield return FieldChecker(x => x.PlaybackTime, nameof(OutputPlaybackData.PlaybackTime));
				yield return FieldChecker(x => x.Song, songsChecker, nameof(OutputPlaybackData.Song));
			}
		}

		public PlaybackDataChecker(IDataChecker<OutputSongData> songsChecker)
		{
			this.songsChecker = songsChecker ?? throw new ArgumentNullException(nameof(songsChecker));
		}
	}
}
