using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.IntegrationTests.Comparers.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Comparers
{
	public class FolderDataComparer : BasicDataComparer<OutputFolderData>, IFolderDataComparer
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

		public FolderDataComparer(IDiscDataComparer discsComparer)
		{
			this.folderCollectionsComparer = new CollectionsComparer<OutputFolderData>(this);
			this.discCollectionsComparer = new CollectionsComparer<OutputDiscData>(discsComparer);
		}
	}
}
