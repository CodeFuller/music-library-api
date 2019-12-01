using System.Collections.Generic;

namespace MusicLibraryApi.Abstractions.Models
{
	public class Folder
	{
		public int Id { get; }

		public string Name { get; }

		public IReadOnlyCollection<Folder>? Subfolders { get; }

		public IReadOnlyCollection<Disc>? Discs { get; }

		public Folder(string name, IReadOnlyCollection<Folder>? subfolders, IReadOnlyCollection<Disc>? discs)
		{
			Name = name;
			Subfolders = subfolders;
			Discs = discs;
		}

		public Folder(int id, string name, IReadOnlyCollection<Folder>? subfolders, IReadOnlyCollection<Disc>? discs)
			: this(name, subfolders, discs)
		{
			Id = id;
		}
	}
}
