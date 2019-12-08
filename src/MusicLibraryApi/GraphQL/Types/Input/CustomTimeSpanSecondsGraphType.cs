using System;
using GraphQL.Types;

namespace MusicLibraryApi.GraphQL.Types.Input
{
	/// <summary>
	/// This class was added to deliver a fix for TimeSpanSecondsGraphType made in scope of https://github.com/graphql-dotnet/graphql-dotnet/pull/1007
	/// When GraphQL 3.0.0 is released with fix included into TimeSpanSecondsGraphType, the class could be removed.
	/// </summary>
	public class CustomTimeSpanSecondsGraphType : TimeSpanSecondsGraphType
	{
		public override object? Serialize(object value)
		{
			return value switch
				{
					TimeSpan timeSpan => (long)timeSpan.TotalSeconds,
					int i => i,
					long l => l,
					_ => (object?)null
				};
		}
	}
}
