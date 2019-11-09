using System;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class DiscInput
	{
		public int? Year { get; set; }

		public string? Title { get; set; }

		public string? AlbumTitle { get; set; }

		public int? AlbumOrder { get; set; }

		public DateTimeOffset? DeleteDate { get; set; }

		public string? DeleteComment { get; set; }

		public Disc ToModel()
		{
			if (Title == null)
			{
				throw new InvalidOperationException("Disc title is not set");
			}

			return new Disc(Year, Title, AlbumTitle, AlbumOrder, DeleteDate, DeleteComment);
		}
	}
}
