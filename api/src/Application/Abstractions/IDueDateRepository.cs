using System;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface IDueDateRepository
    {
        Task<DueDate?> GetByItemIdAsync(Guid itemId, CancellationToken cancellationToken);
        Task<DueDate> UpsertAsync(DueDate dueDate, CancellationToken cancellationToken);
        Task RemoveAsync(Guid itemId, CancellationToken cancellationToken);
    }
}
