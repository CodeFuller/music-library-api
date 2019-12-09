using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class FolderDataChecker : BasicDataChecker<OutputFolderData>
	{
		private readonly IDataChecker<OutputDiscData> discsChecker;

		protected override IEnumerable<Action<OutputFolderData, OutputFolderData, string>> PropertiesCheckers
		{
			get
			{
				yield return FieldChecker(x => x.Id,  nameof(OutputFolderData.Id));
				yield return FieldChecker(x => x.Name, nameof(OutputFolderData.Name));
				yield return FieldChecker(x => x.Subfolders, this, nameof(OutputFolderData.Subfolders));
				yield return FieldChecker(x => x.Discs, discsChecker, nameof(OutputFolderData.Discs));
			}
		}

		public FolderDataChecker(IDataChecker<OutputDiscData> discsChecker)
		{
			this.discsChecker = discsChecker ?? throw new ArgumentNullException(nameof(discsChecker));
		}
	}
}
