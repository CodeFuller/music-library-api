using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Songs;
using MusicLibraryApi.IntegrationTests.Comparers.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class DiscDataComparer : BasicDataComparer<OutputDiscData>, IDiscDataComparer
	{
		private readonly IComparer<IReadOnlyCollection<OutputSongData>?> songCollectionsComparer;

		public IFolderDataComparer? FoldersComparer { get; set; }

		protected override IEnumerable<Func<OutputDiscData, OutputDiscData, int>> PropertyComparers
		{
			get
			{
				if (FoldersComparer == null)
				{
					throw new InvalidOperationException("DiscDataComparer.FoldersComparer is not set");
				}

				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Year);
				yield return FieldComparer(x => x.Title);
				yield return FieldComparer(x => x.TreeTitle);
				yield return FieldComparer(x => x.AlbumTitle);
				yield return FieldComparer(x => x.AlbumId);
				yield return FieldComparer(x => x.AlbumOrder);
				yield return FieldComparer(x => x.DeleteDate);
				yield return FieldComparer(x => x.DeleteComment);
				yield return FieldComparer(x => x.Folder, FoldersComparer);
				yield return FieldComparer(x => x.Songs, songCollectionsComparer);
			}
		}

		public DiscDataComparer(ISongDataComparer songsComparer)
		{
			this.songCollectionsComparer = new CollectionsComparer<OutputSongData>(songsComparer);
		}
	}
}
