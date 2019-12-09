using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Folders;
using MusicLibraryApi.Client.Contracts.Songs;

namespace MusicLibraryApi.Client.Fields
{
	public static class DiscFields
	{
		public static QueryField<OutputDiscData> Id { get; } = new QueryField<OutputDiscData>("id");

		public static QueryField<OutputDiscData> Year { get; } = new QueryField<OutputDiscData>("year");

		public static QueryField<OutputDiscData> Title { get; } = new QueryField<OutputDiscData>("title");

		public static QueryField<OutputDiscData> TreeTitle { get; } = new QueryField<OutputDiscData>("treeTitle");

		public static QueryField<OutputDiscData> AlbumTitle { get; } = new QueryField<OutputDiscData>("albumTitle");

		public static QueryField<OutputDiscData> AlbumId { get; } = new QueryField<OutputDiscData>("albumId");

		public static QueryField<OutputDiscData> AlbumOrder { get; } = new QueryField<OutputDiscData>("albumOrder");

		public static ComplexQueryField<OutputDiscData, OutputSongData> Songs(QueryFieldSet<OutputSongData> songFields)
		{
			return new ComplexQueryField<OutputDiscData, OutputSongData>("songs", songFields);
		}

		public static ComplexQueryField<OutputDiscData, OutputFolderData> Folder(QueryFieldSet<OutputFolderData> folderFields)
		{
			return new ComplexQueryField<OutputDiscData, OutputFolderData>("folder", folderFields);
		}

		public static QueryField<OutputDiscData> DeleteDate { get; } = new QueryField<OutputDiscData>("deleteDate");

		public static QueryField<OutputDiscData> DeleteComment { get; } = new QueryField<OutputDiscData>("deleteComment");

		public static QueryFieldSet<OutputDiscData> All { get; } = Id + Year + Title + TreeTitle + AlbumTitle + AlbumId + AlbumOrder + DeleteDate + DeleteComment;
	}
}
