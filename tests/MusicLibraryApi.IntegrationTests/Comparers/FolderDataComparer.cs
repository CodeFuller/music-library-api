using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class FolderDataComparer : BasicDataComparer<OutputFolderData>
	{
		private readonly IComparer<IReadOnlyCollection<OutputFolderData>?> folderCollectionsComparer;

		private readonly IComparer<IReadOnlyCollection<OutputDiscData>?> discCollectionsComparer;

		protected override IEnumerable<Func<OutputFolderData, OutputFolderData, int>> PropertyComparers
		{
			get
			{
				yield return FieldComparer(x => x.Id);
				yield return FieldComparer(x => x.Name);
				yield return FieldComparer(x => x.Subfolders, folderCollectionsComparer);
				yield return FieldComparer(x => x.Discs, discCollectionsComparer);
			}
		}

		public FolderDataComparer()
		{
			folderCollectionsComparer = new CollectionsComparer<OutputFolderData>(this);
			discCollectionsComparer = new CollectionsComparer<OutputDiscData>(new DiscDataComparer(this));
		}
	}
}
