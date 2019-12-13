using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Output
{
	public class AddPlaybackResultType : ObjectGraphType<AddPlaybackResult>
	{
		public AddPlaybackResultType()
		{
			Field(x => x.NewPlaybackId);
		}
	}
}
