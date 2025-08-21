using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PulseTrack.Application.Abstractions;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Persistence.Repositories
{
    public class ItemHistoryRepository : IItemHistoryRepository
    {
        private readonly AppDbContext _dbContext;

        public ItemHistoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IReadOnlyList<ItemHistory>> ListByItemAsync(
            Guid itemId,
            CancellationToken cancellationToken
        )
        {
            return _dbContext
                .ItemHistories.AsNoTracking()
                .Where(h => h.ItemId == itemId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync(cancellationToken)!
                .ContinueWith(t => (IReadOnlyList<ItemHistory>)t.Result!, cancellationToken);
        }
    }
}


