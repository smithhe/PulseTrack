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
    public class LabelRepository : ILabelRepository
    {
        private readonly AppDbContext _dbContext;

        public LabelRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IReadOnlyList<Label>> ListAsync(CancellationToken cancellationToken)
        {
            return this
                ._dbContext.Labels.AsNoTracking()
                .OrderBy(l => l.Name)
                .ToListAsync(cancellationToken)
                .ContinueWith(t => (IReadOnlyList<Label>)t.Result, cancellationToken);
        }

        public async Task<Label> AddAsync(Label label, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Labels.Add(label);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return label;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public Task<Label?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext
                .Labels.AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Label label, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Labels.Update(label);
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
            Label? entity = await _dbContext.Labels.FirstOrDefaultAsync(
                l => l.Id == id,
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
                _dbContext.Labels.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task AssignAsync(
            Guid itemId,
            Guid labelId,
            CancellationToken cancellationToken
        )
        {
            bool exists = await _dbContext.ItemLabels.AnyAsync(
                il => il.ItemId == itemId && il.LabelId == labelId,
                cancellationToken
            );

            if (exists)
            {
                return;
            }

            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.ItemLabels.Add(new ItemLabel { ItemId = itemId, LabelId = labelId });
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task UnassignAsync(
            Guid itemId,
            Guid labelId,
            CancellationToken cancellationToken
        )
        {
            ItemLabel? link = await _dbContext.ItemLabels.FirstOrDefaultAsync(
                il => il.ItemId == itemId && il.LabelId == labelId,
                cancellationToken
            );

            if (link is null)
            {
                return;
            }

            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.ItemLabels.Remove(link);
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
