using System;
using Microsoft.EntityFrameworkCore;
using PulseTrack.Application.Abstractions;
using PulseTrack.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PulseTrack.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Project>> ListAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Projects.AsNoTracking()
                .OrderBy(p => p.SortOrder)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return project;
        }

        public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Project project, CancellationToken cancellationToken)
        {
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Project? entity = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (entity is null) return;
            _dbContext.Projects.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}


