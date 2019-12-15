using GraphQL.Types;
using MusicLibraryApi.Interfaces;

namespace MusicLibraryApi.GraphQL.Types
{
	public class StatisticsType : ObjectGraphType
	{
		public StatisticsType(IServiceAccessor serviceAccessor)
		{
			FieldAsync<NonNullGraphType<IntGraphType>>("artistsNumber", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetArtistsNumber(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<IntGraphType>>("discArtistsNumber", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetDiscArtistsNumber(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<IntGraphType>>("discsNumber", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetDiscsNumber(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<IntGraphType>>("songsNumber", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetSongsNumber(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<TimeSpanSecondsGraphType>>("songsDuration", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetSongsDuration(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<TimeSpanSecondsGraphType>>("playbacksDuration", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetPlaybacksDuration(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<IntGraphType>>("playbacksNumber", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetPlaybacksNumber(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<IntGraphType>>("unheardSongsNumber", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetUnheardSongsNumber(context.CancellationToken);
			});

			FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<RatingSongsNumberType>>>>("songsRatings", resolve: async context =>
			{
				var statisticsService = serviceAccessor.StatisticsService;
				return await statisticsService.GetSongsRatingsNumbers(context.CancellationToken);
			});
		}
	}
}
