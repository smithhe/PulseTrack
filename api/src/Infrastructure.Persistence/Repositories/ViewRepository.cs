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
    public class ViewRepository : IViewRepository
    {
        private readonly AppDbContext _dbContext;

        public ViewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<View>> ListAsync(
            Guid? projectId,
            CancellationToken cancellationToken
        )
        {
            IQueryable<View> query = _dbContext.Views.AsNoTracking();
            if (projectId.HasValue)
            {
                query = query.Where(v => v.ProjectId == projectId.Value);
            }
            return await query.OrderBy(v => v.Name).ToListAsync(cancellationToken);
        }

        public Task<View?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext
                .Views.AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<View> AddAsync(View view, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Views.Add(view);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return view;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task UpdateAsync(View view, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Views.Update(view);
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
            View? entity = await _dbContext.Views.FirstOrDefaultAsync(
                v => v.Id == id,
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
                _dbContext.Views.Remove(entity);
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
