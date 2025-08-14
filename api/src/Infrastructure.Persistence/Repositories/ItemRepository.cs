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
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _dbContext;

        public ItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Item>> ListAsync(Guid? projectId, CancellationToken cancellationToken)
        {
            IQueryable<Item> query = _dbContext.Items.AsNoTracking();
            if (projectId.HasValue)
            {
                query = query.Where(i => i.ProjectId == projectId.Value);
            }
            return await query.OrderBy(i => i.CreatedAt).ToListAsync(cancellationToken);
        }

        public async Task<Item> AddAsync(Item item, CancellationToken cancellationToken)
        {
            _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return item;
        }

        public Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Item item, CancellationToken cancellationToken)
        {
            _dbContext.Items.Update(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Item? entity = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            if (entity is null) return;
            _dbContext.Items.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}


