using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Client.Fields;
using MusicLibraryApi.Client.Interfaces;

namespace MusicLibraryApi.IntegrationTests.Tests
{
	[TestClass]
	public class DisposalTests : GraphQLTests
	{
		private readonly DisposableUnitOfWork unitOfWorkMock = new DisposableUnitOfWork();

		private sealed class DisposableUnitOfWork : IUnitOfWork, IDisposable
		{
			public bool Disposed { get; set; }

			public IFoldersRepository FoldersRepository => throw new NotImplementedException();

			public IDiscsRepository DiscsRepository => throw new NotImplementedException();

			public IArtistsRepository ArtistsRepository => throw new NotImplementedException();

			public IGenresRepository GenresRepository
			{
				get
				{
					var genresRepositoryStub = new Mock<IGenresRepository>();
					genresRepositoryStub.Setup(x => x.GetAllGenres(It.IsAny<CancellationToken>())).ReturnsAsync(Array.Empty<Genre>());

					return genresRepositoryStub.Object;
				}
			}

			public ISongsRepository SongsRepository => throw new NotImplementedException();

			public IPlaybacksRepository PlaybacksRepository => throw new NotImplementedException();

			public Task Commit(CancellationToken cancellationToken)
			{
				throw new NotImplementedException();
			}

			public void Dispose()
			{
				Disposed = true;
			}
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);

			services.AddTransient<IUnitOfWork>(sp => unitOfWorkMock);
		}

		// This test was added after a defect when all instances of MusicLibraryDbContext were disposed only on application exit.
		// There is no easy way to mock MusicLibraryDbContext and verify it's disposal.
		// That's why we verify dispose of UnitOfWork instance. That's effectively the same since instances are disposed by DI container.
		// The test verifies dispose()
		[TestMethod]
		public async Task DbContext_AfterRequest_IsDisposed()
		{
			// Arrange

			var client = CreateClient<IGenresQuery>();

			unitOfWorkMock.Disposed = false;

			// Act

			await client.GetGenres(GenreFields.All, CancellationToken.None);

			// Assert

			Assert.IsTrue(unitOfWorkMock.Disposed);
		}
	}
}
