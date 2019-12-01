using System.Collections.Generic;
using System.Runtime.Serialization;
using MusicLibraryApi.Client.Contracts.Discs;

namespace MusicLibraryApi.Client.Contracts.Folders
{
	[DataContract]
	public class OutputFolderData : BasicFolderData
	{
		[DataMember(Name = "id")]
		public int? Id { get; }

		[DataMember(Name = "subfolders")]
		public IReadOnlyCollection<OutputFolderData>? Subfolders { get; }

		[DataMember(Name = "discs")]
		public IReadOnlyCollection<OutputDiscData>? Discs { get; }

		public OutputFolderData(int? id, string name, IReadOnlyCollection<OutputFolderData>? subfolders = null, IReadOnlyCollection<OutputDiscData>? discs = null)
			: base(name)
		{
			Id = id;
			Subfolders = subfolders;
			Discs = discs;
		}
	}
}
