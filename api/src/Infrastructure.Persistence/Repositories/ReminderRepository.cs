using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PulseTrack.Application.Abstractions;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Persistence.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly AppDbContext _dbContext;

        public ReminderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IReadOnlyList<Reminder>> ListByItemAsync(
            Guid itemId,
            CancellationToken cancellationToken
        )
        {
            return this
                ._dbContext.Reminders.AsNoTracking()
                .Where(r => r.ItemId == itemId)
                .OrderBy(r => r.RemindAtUtc)
                .ToListAsync(cancellationToken)
                .ContinueWith(t => (IReadOnlyList<Reminder>)t.Result, cancellationToken);
        }

        public async Task<Reminder> AddAsync(Reminder reminder, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Reminders.Add(reminder);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return reminder;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public Task<Reminder?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext
                .Reminders.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Reminder reminder, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Reminders.Update(reminder);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Reminder? entity = await _dbContext.Reminders.FirstOrDefaultAsync(
                r => r.Id == id,
                cancellationToken
            );
            if (entity is null)
            {
                return;
            }

            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Reminders.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
