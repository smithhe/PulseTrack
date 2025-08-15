using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Abstractions
{
    public interface ISectionRepository
    {
        Task<IReadOnlyList<Section>> ListByProjectAsync(
            Guid projectId,
            CancellationToken cancellationToken
        );
        Task<Section> AddAsync(Section section, CancellationToken cancellationToken);
        Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Section section, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task ReorderAsync(
            Guid projectId,
            IReadOnlyList<Guid> orderedSectionIds,
            CancellationToken cancellationToken
        );
    }
}
