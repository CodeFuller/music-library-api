using System;
using System.Collections.Generic;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.IntegrationTests.DataCheckers
{
	public class DiscDataChecker : BasicDataChecker<OutputDiscData>
	{
		private readonly IDataChecker<OutputSongData> songsChecker;

		public IDataChecker<OutputFolderData>? FoldersChecker { get; set; }

		protected override IEnumerable<Action<OutputDiscData, OutputDiscData, string>> PropertiesCheckers
		{
			get
			{
				if (FoldersChecker == null)
				{
					throw new InvalidOperationException($"{nameof(DiscDataChecker)}.{nameof(FoldersChecker)} is not set");
				}

				yield return FieldChecker(x => x.Id, nameof(OutputDiscData.Id));
				yield return FieldChecker(x => x.Year, nameof(OutputDiscData.Year));
				yield return FieldChecker(x => x.Title, nameof(OutputDiscData.Title));
				yield return FieldChecker(x => x.TreeTitle, nameof(OutputDiscData.TreeTitle));
				yield return FieldChecker(x => x.AlbumTitle, nameof(OutputDiscData.AlbumTitle));
				yield return FieldChecker(x => x.AlbumId, nameof(OutputDiscData.AlbumId));
				yield return FieldChecker(x => x.AlbumOrder, nameof(OutputDiscData.AlbumOrder));
				yield return FieldChecker(x => x.DeleteDate, nameof(OutputDiscData.DeleteDate));
				yield return FieldChecker(x => x.DeleteComment, nameof(OutputDiscData.DeleteComment));
				yield return FieldChecker(x => x.Folder, FoldersChecker, nameof(OutputDiscData.Folder));
				yield return FieldChecker(x => x.Songs, songsChecker, nameof(OutputDiscData.Songs));
			}
		}

		public DiscDataChecker(IDataChecker<OutputSongData> songsChecker)
		{
			this.songsChecker = songsChecker ?? throw new ArgumentNullException(nameof(songsChecker));
		}
	}
}
