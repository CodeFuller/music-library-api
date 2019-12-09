using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Artists;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class SongDataChecker : BasicDataChecker<OutputSongData>
	{
		public IDataChecker<OutputDiscData>? DiscsChecker { get; set; }

		public IDataChecker<OutputArtistData>? ArtistsChecker { get; set; }

		public IDataChecker<OutputGenreData>? GenresChecker { get; set; }

		protected override IEnumerable<Action<OutputSongData, OutputSongData, string>> PropertiesCheckers
		{
			get
			{
				if (DiscsChecker == null)
				{
					throw new InvalidOperationException($"{nameof(SongDataChecker)}.{nameof(DiscsChecker)}  is not set");
				}

				if (ArtistsChecker == null)
				{
					throw new InvalidOperationException($"{nameof(SongDataChecker)}.{nameof(ArtistsChecker)}  is not set");
				}

				if (GenresChecker == null)
				{
					throw new InvalidOperationException($"{nameof(SongDataChecker)}.{nameof(GenresChecker)}  is not set");
				}

				yield return FieldChecker(s => s.Id, nameof(OutputSongData.Id));
				yield return FieldChecker(s => s.Title, nameof(OutputSongData.Title));
				yield return FieldChecker(s => s.TreeTitle, nameof(OutputSongData.TreeTitle));
				yield return FieldChecker(s => s.TrackNumber, nameof(OutputSongData.TrackNumber));
				yield return FieldChecker(s => s.Duration, nameof(OutputSongData.Duration));
				yield return FieldChecker(s => s.Disc, DiscsChecker, nameof(OutputSongData.Disc));
				yield return FieldChecker(s => s.Artist, ArtistsChecker, nameof(OutputSongData.Artist));
				yield return FieldChecker(s => s.Genre, GenresChecker, nameof(OutputSongData.Genre));
				yield return FieldChecker(s => s.Rating, nameof(OutputSongData.Rating));
				yield return FieldChecker(s => s.BitRate, nameof(OutputSongData.BitRate));
				yield return FieldChecker(s => s.LastPlaybackTime, nameof(OutputSongData.LastPlaybackTime));
				yield return FieldChecker(s => s.PlaybacksCount, nameof(OutputSongData.PlaybacksCount));
				yield return FieldChecker(s => s.DeleteDate, nameof(OutputSongData.DeleteDate));
				yield return FieldChecker(s => s.DeleteComment, nameof(OutputSongData.DeleteComment));
			}
		}
	}
}
