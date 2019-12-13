using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Playbacks
{
	[DataContract]
	public class AddPlaybackOutputData
	{
		[DataMember(Name = "newPlaybackId")]
		public int? NewPlaybackId { get; }

		public AddPlaybackOutputData(int? newPlaybackId)
		{
			NewPlaybackId = newPlaybackId;
		}
	}
}
