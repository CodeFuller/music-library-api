using System;
using Newtonsoft.Json;

namespace MusicLibraryApi.Client.Converters
{
	internal class TimespanSecondsConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TimeSpan);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return null;
			}

			if (reader.Value is int i)
			{
				return TimeSpan.FromSeconds(i);
			}

			if (reader.Value is long l)
			{
				return TimeSpan.FromSeconds(l);
			}

			throw new InvalidOperationException($"Unexpected type of input TimeSpan value: {reader.Value.GetType()}");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (!(value is TimeSpan timeSpan))
			{
				throw new InvalidOperationException($"Unexpected type of output TimeSpan value: {value.GetType()}");
			}

			writer.WriteValue((long)timeSpan.TotalSeconds);
		}
	}
}
