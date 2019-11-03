using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class DiscsRepository : IDiscsRepository
	{
		private readonly MusicLibraryDbContext context;

		private IQueryable<Disc> DiscsWithIncludedProperties =>
			context.Discs
				.Include(d => d.Songs).ThenInclude(s => s.Genre)
				.Include(d => d.Songs).ThenInclude(s => s.Artist);

		public DiscsRepository(MusicLibraryDbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Disc>> GetAllDiscs(CancellationToken cancellationToken)
		{
			return await DiscsWithIncludedProperties
				.ToListAsync(cancellationToken);
		}

		public async Task<Disc> GetDisc(int discId, CancellationToken cancellationToken)
		{
			return await DiscsWithIncludedProperties
				.Where(x => x.Id == discId)
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
