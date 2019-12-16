using System;
using MusicLibraryApi.Client.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicLibraryApi.Client.Converters
{
	internal class CustomContractResolver : DefaultContractResolver
	{
		protected override JsonConverter ResolveContractConverter(Type objectType)
		{
			if (objectType == typeof(TimeSpan))
			{
				return new TimespanSecondsConverter();
			}

			if (objectType == typeof(Rating))
			{
				return new RatingConverter();
			}

			return base.ResolveContractConverter(objectType);
		}
	}
}
