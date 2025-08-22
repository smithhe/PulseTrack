using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface IViewRepository
    {
        Task<IReadOnlyList<View>> ListAsync(Guid? projectId, CancellationToken cancellationToken);
        Task<View?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<View> AddAsync(View view, CancellationToken cancellationToken);
        Task UpdateAsync(View view, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
