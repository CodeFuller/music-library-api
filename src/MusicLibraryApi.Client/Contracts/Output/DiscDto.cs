using System;

namespace MusicLibraryApi.Client.Contracts.Output
{
	public class DiscDto
	{
		public int? Id { get; }

		public int? Year { get; }

		public string? Title { get; }

		public string? AlbumTitle { get; }

		public int? AlbumOrder { get; }

		public DateTimeOffset? DeleteDate { get; }

		public string? DeleteComment { get; }

		public DiscDto(int? id, int? year, string? title, string? albumTitle, int? albumOrder, DateTimeOffset? deleteDate, string? deleteComment)
		{
			Id = id;
			Year = year;
			Title = title;
			AlbumTitle = albumTitle;
			AlbumOrder = albumOrder;
			DeleteDate = deleteDate;
			DeleteComment = deleteComment;
		}
	}
}
