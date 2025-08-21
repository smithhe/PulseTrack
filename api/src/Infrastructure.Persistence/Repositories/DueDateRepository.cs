using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PulseTrack.Application.Abstractions;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Persistence.Repositories
{
    public class DueDateRepository : IDueDateRepository
    {
        private readonly AppDbContext _dbContext;

        public DueDateRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<DueDate?> GetByItemIdAsync(Guid itemId, CancellationToken cancellationToken)
        {
            return _dbContext
                .DueDates.AsNoTracking()
                .FirstOrDefaultAsync(d => d.ItemId == itemId, cancellationToken);
        }

        public async Task<DueDate> UpsertAsync(DueDate dueDate, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                DueDate? existing = await _dbContext.DueDates.FirstOrDefaultAsync(
                    d => d.ItemId == dueDate.ItemId,
                    cancellationToken
                );

                if (existing is null)
                {
                    _dbContext.DueDates.Add(dueDate);
                }
                else
                {
                    existing.DateUtc = dueDate.DateUtc;
                    existing.Timezone = dueDate.Timezone;
                    existing.IsRecurring = dueDate.IsRecurring;
                    existing.RecurrenceType = dueDate.RecurrenceType;
                    existing.RecurrenceInterval = dueDate.RecurrenceInterval;
                    existing.RecurrenceCount = dueDate.RecurrenceCount;
                    existing.RecurrenceEndUtc = dueDate.RecurrenceEndUtc;
                    existing.RecurrenceWeeks = dueDate.RecurrenceWeeks;
                    _dbContext.DueDates.Update(existing);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return dueDate;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task RemoveAsync(Guid itemId, CancellationToken cancellationToken)
        {
            DueDate? existing = await _dbContext.DueDates.FirstOrDefaultAsync(
                d => d.ItemId == itemId,
                cancellationToken
            );

            if (existing is null)
            {
                return;
            }

            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                _dbContext.DueDates.Remove(existing);
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
