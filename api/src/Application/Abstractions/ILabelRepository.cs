using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface ILabelRepository
    {
        Task<IReadOnlyList<Label>> ListAsync(CancellationToken cancellationToken);
        Task<Label> AddAsync(Label label, CancellationToken cancellationToken);
        Task<Label?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Label label, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task AssignAsync(Guid itemId, Guid labelId, CancellationToken cancellationToken);
        Task UnassignAsync(Guid itemId, Guid labelId, CancellationToken cancellationToken);
    }
}
