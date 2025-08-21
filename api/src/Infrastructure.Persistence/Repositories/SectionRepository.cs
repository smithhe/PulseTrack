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
    public class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _dbContext;

        public SectionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IReadOnlyList<Section>> ListByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken
        )
        {
            return this
                ._dbContext.Sections.AsNoTracking()
                .Where(s => s.ProjectId == projectId)
                .OrderBy(s => s.SortOrder)
                .ToListAsync(cancellationToken)
                .ContinueWith(t => (IReadOnlyList<Section>)t.Result, cancellationToken);
        }

        public async Task<Section> AddAsync(Section section, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Sections.Add(section);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return section;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext
                .Sections.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Section section, CancellationToken cancellationToken)
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _dbContext.Sections.Update(section);
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
            Section? entity = await _dbContext.Sections.FirstOrDefaultAsync(
                s => s.Id == id,
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
                _dbContext.Sections.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task ReorderAsync(
            Guid projectId,
            IReadOnlyList<Guid> orderedSectionIds,
            CancellationToken cancellationToken
        )
        {
            await using IDbContextTransaction transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                List<Section> sections = await _dbContext
                    .Sections.Where(s => s.ProjectId == projectId)
                    .ToListAsync(cancellationToken);

                Dictionary<Guid, int> orderMap = orderedSectionIds
                    .Select((id, idx) => new { id, idx })
                    .ToDictionary(x => x.id, x => x.idx);

                foreach (Section s in sections)
                {
                    if (orderMap.TryGetValue(s.Id, out int idx))
                    {
                        s.SortOrder = idx;
                    }
                }

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
