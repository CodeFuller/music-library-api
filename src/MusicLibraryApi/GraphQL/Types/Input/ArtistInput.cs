using System;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	public class ArtistInput
	{
		public string? Name { get; set; }

		public Artist ToModel()
		{
			if (Name == null)
			{
				throw new InvalidOperationException("Artist name is not set");
			}

			return new Artist
			{
				Name = Name,
			};
		}
	}
}
