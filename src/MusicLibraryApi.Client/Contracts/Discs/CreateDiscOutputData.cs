using System.Runtime.Serialization;

namespace MusicLibraryApi.Client.Contracts.Discs
{
	[DataContract]
	public class CreateDiscOutputData
	{
		[DataMember(Name = "newDiscId")]
		public int? NewDiscId { get; }

		public CreateDiscOutputData(int? newDiscId)
		{
			NewDiscId = newDiscId;
		}
	}
}
