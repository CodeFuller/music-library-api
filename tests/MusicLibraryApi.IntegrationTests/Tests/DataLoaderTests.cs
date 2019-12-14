using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;
using MusicLibraryApi.Logic.Services;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class DataLoaderTests : GraphQLTests
	{
		private ArtistsServiceMock? ArtistsService { get; set; }

		private DiscsServiceMock? DiscsService { get; set; }

		private abstract class BasicServiceMock<TService>
			where TService : class
		{
			private readonly ConcurrentDictionary<string, int> calls = new ConcurrentDictionary<string, int>();

			private readonly TService innerService;

			protected BasicServiceMock(TService innerService)
			{
				this.innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
			}

			protected T RegisterCall<T>(Func<TService, T> call, [CallerMemberName] string? caller = null)
			{
				var callId = $"{innerService.GetType().Name}.{caller}";
				calls.AddOrUpdate(callId, k => 1, (k, v) => v + 1);

				return call(innerService);
			}

			public int GetCallsNumber(string callId)
			{
				return calls.TryGetValue(callId, out var callsNumber) ? callsNumber : 0;
			}
		}

		private class ArtistsServiceMock : BasicServiceMock<IArtistsService>, IArtistsService
		{
			public ArtistsServiceMock(IArtistsService innerService)
				: base(innerService)
			{
			}

			public Task<int> CreateArtist(Artist artist, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.CreateArtist(artist, cancellationToken));
			}

			public Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetAllArtists(cancellationToken));
			}

			public Task<IDictionary<int, Artist>> GetArtists(IEnumerable<int> artistIds, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetArtists(artistIds, cancellationToken));
			}

			public Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetArtist(artistId, cancellationToken));
			}
		}

		private class DiscsServiceMock : BasicServiceMock<IDiscsService>, IDiscsService
		{
			public DiscsServiceMock(IDiscsService innerService)
				: base(innerService)
			{
			}

			public Task<int> CreateDisc(Disc disc, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.CreateDisc(disc, cancellationToken));
			}

			public Task<IReadOnlyCollection<Disc>> GetAllDiscs(CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetAllDiscs(cancellationToken));
			}

			public Task<IDictionary<int, Disc>> GetDiscs(IEnumerable<int> discIds, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetDiscs(discIds, cancellationToken));
			}

			public Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetDisc(discId, cancellationToken));
			}

			public Task<ILookup<int, Disc>> GetDiscsByFolderIds(IEnumerable<int> folderIds, bool includeDeletedDiscs, CancellationToken cancellationToken)
			{
				return RegisterCall(service => service.GetDiscsByFolderIds(folderIds, includeDeletedDiscs, cancellationToken));
			}
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);

			services.AddSingleton<IArtistsService>(sp =>
			{
				ArtistsService = new ArtistsServiceMock(new ArtistsService(sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<ILogger<ArtistsService>>()));
				return ArtistsService;
			});

			services.AddSingleton<IDiscsService>(sp =>
			{
				DiscsService = new DiscsServiceMock(new DiscsService(sp.GetRequiredService<IUnitOfWork>(), sp.GetRequiredService<ILogger<DiscsService>>()));
				return DiscsService;
			});
		}

		[TestMethod]
		public async Task GraphQl_ForNestedQueries_UsesDataLoaderWithBatching()
		{
			// Arrange

			var requestedFields = FolderFields.Name + FolderFields.Discs(DiscFields.Songs(SongFields.Id + SongFields.Artist(ArtistFields.Id + ArtistFields.Name)));

			var client = CreateClient<IFoldersQuery>();

			// Act

			var folderData = await client.GetFolder(null, requestedFields, CancellationToken.None);

			// Assert

			// Sanity check that multiple results were processed.
			Assert.AreEqual(2, folderData.Discs?.Count);
			var songs = folderData.Discs?.SelectMany(d => d.Songs).ToList();
			Assert.AreEqual(2, songs?.Select(s => s.Artist?.Id).Where(id => id != null).Distinct().Count());

			Assert.AreEqual(1, ArtistsService?.GetCallsNumber("ArtistsService.GetArtists"));
			Assert.AreEqual(1, DiscsService?.GetCallsNumber("DiscsService.GetDiscsByFolderIds"));
		}
	}
}
