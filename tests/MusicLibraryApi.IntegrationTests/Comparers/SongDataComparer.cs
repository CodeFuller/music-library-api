using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class SongDataComparer : BasicDataComparer<OutputSongData>
	{
		protected override IEnumerable<Func<OutputSongData, OutputSongData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(s => s.Id);
				yield return FieldComparer(s => s.Title);
				yield return FieldComparer(s => s.TreeTitle);
				yield return FieldComparer(s => s.TrackNumber);
				yield return FieldComparer(s => s.Duration);
				yield return FieldComparer(s => s.Rating);
				yield return FieldComparer(s => s.BitRate);
				yield return FieldComparer(s => s.LastPlaybackTime);
				yield return FieldComparer(s => s.PlaybacksCount);
				yield return FieldComparer(s => s.DeleteDate);
				yield return FieldComparer(s => s.DeleteComment);
			}
		}
	}
}
