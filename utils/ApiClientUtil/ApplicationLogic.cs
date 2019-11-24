using System;
using System.Threading;
using System.Threading.Tasks;
using CF.Library.Bootstrap;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client;
using MusicLibraryApi.Client.Contracts.Discs;
using MusicLibraryApi.Client.Contracts.Genres;
using MusicLibraryApi.Client.Interfaces;

namespace ApiClientUtil
{
	public class ApplicationLogic : IApplicationLogic
	{
		private readonly IGenresQuery genresQuery;
		private readonly IGenresMutation genresMutation;
		private readonly IDiscsQuery discsQuery;
		private readonly IDiscsMutation discsMutation;
		private readonly ILogger<ApplicationLogic> logger;

		public ApplicationLogic(IGenresQuery genresQuery, IGenresMutation genresMutation, IDiscsQuery discsQuery, IDiscsMutation discsMutation, ILogger<ApplicationLogic> logger)
		{
			this.genresQuery = genresQuery ?? throw new ArgumentNullException(nameof(genresQuery));
			this.genresMutation = genresMutation ?? throw new ArgumentNullException(nameof(genresMutation));
			this.discsQuery = discsQuery ?? throw new ArgumentNullException(nameof(discsQuery));
			this.discsMutation = discsMutation ?? throw new ArgumentNullException(nameof(discsMutation));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> Run(string[] args, CancellationToken cancellationToken)
		{
			var newDisc = new InputDiscData(1997, "Proud Like A God", null, null, null, null);
			var newDiscId = await discsMutation.CreateDisc(1, newDisc, cancellationToken);

			var newGenre = new InputGenreData($"Some Genre - {DateTimeOffset.Now:yyyy/MM/dd HH:mm:ss}");
			var newGenreId = await genresMutation.CreateGenre(newGenre, cancellationToken);

			foreach (var genre in await genresQuery.GetGenres(GenreFields.All, cancellationToken))
			{
				logger.LogInformation("Loaded genre: {GenreId} - {GenreName}", genre.Id, genre.Name);
			}

			var disc = await discsQuery.GetDisc(2, DiscFields.All, cancellationToken);
			logger.LogInformation("Loaded specific disc: {DiscId} - {DiscTitle}", disc.Id, disc.Title);

			foreach (var currDisc in await discsQuery.GetDiscs(DiscFields.All, cancellationToken))
			{
				logger.LogInformation("Loaded disc: {DiscId} - {DiscTitle}", currDisc.Id, currDisc.Title);
			}

			return 0;
		}
	}
}
