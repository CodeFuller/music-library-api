using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using TagLib;

namespace MusicLibraryApi.Tagger
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class SongTagger : ISongTagger
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		static SongTagger()
		{
			// Avoiding default usage of numeric genre code.
			// Otherwise we can't reach exact genre requested by the user.
			// For example 'Alternative Rock' will result in 'AlternRock' for numeric genres mode.
			TagLib.Id3v2.Tag.UseNumericGenres = false;
		}

		public Task SetTagData(Song song, Disc disc, Artist? artist, Genre? genre, Stream contentStream, CancellationToken cancellationToken)
		{
			var streamWrapper = new StreamFileAbstraction(contentStream, song.TreeTitle);

			try
			{
				using var file = TagLib.File.Create(streamWrapper);
				SetTagData(song, disc, artist, genre, file);
			}
			catch (CorruptFileException e)
			{
				throw new InvalidSongContentException("Song content is corrupted", e);
			}

			return Task.CompletedTask;
		}

		private static void SetTagData(Song song, Disc disc, Artist? artist, Genre? genre, TagLib.File file)
		{
			file.RemoveTags(TagTypes.Id3v1);
			file.RemoveTags(TagTypes.Id3v2);
			file.RemoveTags(TagTypes.Ape);

			// We set only Id3v2 tag.
			// Id3v1 stores genres as index from predefined genre list.
			// This list is pretty limited and doesn't contain frequently used tags like 'Symphonic Metal' or 'Nu metal'.
			// The list of available genres for Id3v2 tag could be obtained from TagLib.Genres.Audio.
			var tag = file.GetTag(TagTypes.Id3v2, true);
			FillTag(song, disc, artist, genre, tag);
			file.Save();
		}

		private static void FillTag(Song song, Disc disc, Artist? artist, Genre? genre, Tag tag)
		{
			// Frankly, the tag fields are written to the file in the order they are set on the Tag object.
			// This does not matter for the functional aspect.
			// However, the files will differ in binary content. Checksum will also differ.
			// That is why it's better not to change the order without the need.
			if (artist != null)
			{
				tag.Performers = new[] { artist.Name };
			}

			tag.Album = disc.AlbumTitle;

			if (disc.Year.HasValue)
			{
				tag.Year = (uint)disc.Year;
			}

			if (genre != null)
			{
				tag.Genres = new[] { genre.Name };
			}

			if (song.TrackNumber.HasValue)
			{
				tag.Track = (uint)song.TrackNumber;
			}

			tag.Title = song.Title;
		}
	}
}
