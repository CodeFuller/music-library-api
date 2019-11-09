using System;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class GenreInput
	{
		public string? Name { get; set; }

		public Genre ToModel()
		{
			if (Name == null)
			{
				throw new InvalidOperationException("Genre name is not set");
			}

			return new Genre(Name);
		}
	}
}
