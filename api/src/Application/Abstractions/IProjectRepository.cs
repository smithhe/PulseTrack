using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface IProjectRepository
    {
        Task<IReadOnlyList<Project>> ListAsync(CancellationToken cancellationToken);
        Task<Project> AddAsync(Project project, CancellationToken cancellationToken);
        Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Project project, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}


