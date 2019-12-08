using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class DiscDataComparer : BasicDataComparer<OutputDiscData>
	{
		private readonly IComparer<IReadOnlyCollection<OutputSongData>?> songCollectionsComparer = new CollectionsComparer<OutputSongData>(new SongDataComparer());

		private readonly FolderDataComparer foldersComparer;

		public DiscDataComparer()
			: this(new FolderDataComparer())
		{
		}

		public DiscDataComparer(FolderDataComparer foldersComparer)
		{
			this.foldersComparer = foldersComparer ?? throw new ArgumentNullException(nameof(foldersComparer));
		}

		protected override IEnumerable<Func<OutputDiscData, OutputDiscData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Year);
				yield return FieldComparer(x => x.Title);
				yield return FieldComparer(x => x.TreeTitle);
				yield return FieldComparer(x => x.AlbumTitle);
				yield return FieldComparer(x => x.AlbumId);
				yield return FieldComparer(x => x.AlbumOrder);
				yield return FieldComparer(x => x.Folder, foldersComparer);
				yield return FieldComparer(x => x.DeleteDate);
				yield return FieldComparer(x => x.DeleteComment);
				yield return FieldComparer(x => x.Songs, songCollectionsComparer);
				yield return FieldComparer(x => x.Year);
				yield return FieldComparer(x => x.Year);
			}
		}
	}
}
