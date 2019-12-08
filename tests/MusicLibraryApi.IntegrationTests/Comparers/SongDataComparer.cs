using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.IntegrationTests.Comparers.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class SongDataComparer : BasicDataComparer<OutputSongData>, ISongDataComparer
	{
		public IDiscDataComparer? DiscsComparer { get; set; }

		public IArtistDataComparer? ArtistsComparer { get; set; }

		public IGenreDataComparer? GenresComparer { get; set; }

		protected override IEnumerable<Func<OutputSongData, OutputSongData, int>> PropertyComparers
		{
			get
			{
				if (DiscsComparer == null)
				{
					throw new InvalidOperationException("SongDataComparer.DiscCollectionsComparer is not set");
				}

				if (ArtistsComparer == null)
				{
					throw new InvalidOperationException("SongDataComparer.ArtistsComparer is not set");
				}

				if (GenresComparer == null)
				{
					throw new InvalidOperationException("SongDataComparer.GenresComparer is not set");
				}

				yield return FieldComparer(s => s.Id);
				yield return FieldComparer(s => s.Title);
				yield return FieldComparer(s => s.TreeTitle);
				yield return FieldComparer(s => s.TrackNumber);
				yield return FieldComparer(s => s.Duration);
				yield return FieldComparer(s => s.Disc, DiscsComparer);
				yield return FieldComparer(s => s.Artist, ArtistsComparer);
				yield return FieldComparer(s => s.Genre, GenresComparer);
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
