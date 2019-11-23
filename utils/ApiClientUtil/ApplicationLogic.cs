using System;
using System.Threading;
using System.Threading.Tasks;
using CF.Library.Bootstrap;
using Microsoft.Extensions.Logging;
using MusicLibraryApi.Client;
using MusicLibraryApi.Client.Interfaces;

namespace ApiClientUtil
{
	public class ApplicationLogic : IApplicationLogic
	{
		private readonly IGenresQuery genresQuery;
		private readonly IDiscsQuery discsQuery;
		private readonly ILogger<ApplicationLogic> logger;

		public ApplicationLogic(IGenresQuery genresQuery, IDiscsQuery discsQuery, ILogger<ApplicationLogic> logger)
		{
			this.genresQuery = genresQuery ?? throw new ArgumentNullException(nameof(genresQuery));
			this.discsQuery = discsQuery ?? throw new ArgumentNullException(nameof(discsQuery));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> Run(string[] args, CancellationToken cancellationToken)
		{
			await foreach (var genre in genresQuery.GetGenres(GenreFields.All, cancellationToken))
			{
				logger.LogInformation("Loaded genre: {GenreId} - {GenreName}", genre.Id, genre.Name);
			}

			var disc = await discsQuery.GetDisc(2, DiscFields.All, cancellationToken);
			logger.LogInformation("Loaded specific disc: {DiscId} - {DiscTitle}", disc.Id, disc.Title);

			await foreach (var currDisc in discsQuery.GetDiscs(DiscFields.All, cancellationToken))
			{
				logger.LogInformation("Loaded disc: {DiscId} - {DiscTitle}", currDisc.Id, currDisc.Title);
			}

			return 0;
		}
	}
}
