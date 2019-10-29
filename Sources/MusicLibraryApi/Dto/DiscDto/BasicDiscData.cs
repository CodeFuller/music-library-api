using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MusicLibraryApi.Dto.DiscDto
{
	[DataContract]
	public abstract class BasicDiscData
	{
		[DataMember(Name = "year")]
		public int? Year { get; set; }

		[Required]
		[DataMember(Name = "title")]
		public string Title { get; set; }

		[DataMember(Name = "albumTitle")]
		public string AlbumTitle { get; set; }

		[DataMember(Name = "albumOrder")]
		public int? AlbumOrder { get; set; }
	}
}
