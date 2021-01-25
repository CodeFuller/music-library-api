using System;
using System.Collections.Generic;
using System.Linq;
using MusicLibraryApi.Client.Contracts;
using Newtonsoft.Json;

namespace MusicLibraryApi.Client.Converters
{
	internal class RatingConverter : JsonConverter
	{
		private readonly Dictionary<string, Rating> stringToRatingMapping;
		private readonly Dictionary<Rating, string> ratingToStringMapping;

		public RatingConverter()
		{
			var ratings = new[]
			{
				("R_1", Rating.R1),
				("R_2", Rating.R2),
				("R_3", Rating.R3),
				("R_4", Rating.R4),
				("R_5", Rating.R5),
				("R_6", Rating.R6),
				("R_7", Rating.R7),
				("R_8", Rating.R8),
				("R_9", Rating.R9),
				("R_10", Rating.R10),
			};

			stringToRatingMapping = ratings.ToDictionary(p => p.Item1, p => p.Item2);
			ratingToStringMapping = ratings.ToDictionary(p => p.Item2, p => p.Item1);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Rating);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return null;
			}

			if (reader.Value is not string ratingValue)
			{
				throw new InvalidOperationException($"Unexpected type of input rating value: {reader.Value.GetType()}");
			}

			if (stringToRatingMapping.TryGetValue(ratingValue, out var rating))
			{
				return rating;
			}

			throw new InvalidOperationException($"Can not deserialize rating value '{ratingValue}'");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (value is not Rating rating)
			{
				throw new InvalidOperationException($"Unexpected type of output rating value: {value.GetType()}");
			}

			if (!ratingToStringMapping.TryGetValue(rating, out var ratingValue))
			{
				throw new InvalidOperationException($"Can not serialize rating value '{rating}'");
			}

			writer.WriteValue(ratingValue);
		}
	}
}
