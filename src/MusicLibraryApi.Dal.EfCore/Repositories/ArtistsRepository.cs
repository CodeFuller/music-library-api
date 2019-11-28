﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicLibraryApi.Abstractions.Exceptions;
using MusicLibraryApi.Abstractions.Interfaces;
using MusicLibraryApi.Abstractions.Models;
using MusicLibraryApi.Dal.EfCore.Entities;

namespace MusicLibraryApi.Dal.EfCore.Repositories
{
	public class ArtistsRepository : IArtistsRepository
	{
		private readonly MusicLibraryDbContext context;

		private readonly IMapper mapper;

		public ArtistsRepository(MusicLibraryDbContext context, IMapper mapper)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<int> CreateArtist(Artist artist, CancellationToken cancellationToken)
		{
			var artistEntity = mapper.Map<ArtistEntity>(artist);

			context.Artists.Add(artistEntity);

			try
			{
				await context.SaveChangesAsync(cancellationToken);
			}
			catch (DbUpdateException e) when (e.IsUniqueViolationException())
			{
				throw new DuplicateKeyException($"Failed to add artist '{artist.Name}' to the database", e);
			}

			return artistEntity.Id;
		}

		public async Task<IReadOnlyCollection<Artist>> GetAllArtists(CancellationToken cancellationToken)
		{
			return await context.Artists
				.OrderBy(a => a.Name)
				.Select(a => mapper.Map<Artist>(a))
				.ToListAsync(cancellationToken);
		}
	}
}
